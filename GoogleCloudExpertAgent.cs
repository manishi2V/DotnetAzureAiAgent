using Azure;
using Azure.Identity;
using Azure.AI.Projects;
using Azure.AI.Agents.Persistent;


namespace AzureAiGoogleAgent
{
    internal class GoogleCloudExpertAgent
    {
        public static async Task Start()
        {
            await RunAgentConversation();
        }
        static async Task RunAgentConversation()
        {
            /* Open command prompt and run below commands with your end point and agent id
            setx AZURE_OPENAI_API_ENDPOINT "https://xxxxxxxxxxxxxxx.openai.azure.com/"
            setz GOOGLE_CLOUD_EXPERT_AGENT_ID = "you agent id present in your Azure Foundry AI Agent Project"
            */

            var openAiEndPoint = Environment.GetEnvironmentVariable("AZURE_OPENAI_API_ENDPOINT");

            if (openAiEndPoint == null)
            {
                Console.WriteLine("Please first set your api end point in environment variable");
                return;
            }

            var endpoint = new Uri(openAiEndPoint);

            AIProjectClient projectClient = new(endpoint, new DefaultAzureCredential());

            var googleCloudExpertAgentId = Environment.GetEnvironmentVariable("GOOGLE_CLOUD_EXPERT_AGENT_ID");

            if (googleCloudExpertAgentId == null)
            {
                Console.WriteLine("Please first set agent Id in environment variable");
                return;
            }
            try
            {
                PersistentAgentsClient agentsClient = projectClient.GetPersistentAgentsClient();

                PersistentAgent googleCloudExpertAgent = agentsClient.Administration.GetAgent(googleCloudExpertAgentId);

                Console.ForegroundColor = ConsoleColor.DarkYellow;

                Console.WriteLine("Available threads:");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Pageable<PersistentAgentThread> threads = agentsClient.Threads.GetThreads();
                foreach (PersistentAgentThread thread in threads)
                {
                    Console.WriteLine($"Thread ID: {thread.Id}, Created on : {thread.CreatedAt}");
                }

                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Enter the thread ID you want to use (or press Enter to create a new one):");
                Console.ForegroundColor = ConsoleColor.Red;
                string? threadId = Console.ReadLine()?.Trim();
                PersistentAgentThread? persistentThread = null;

                Console.ForegroundColor = ConsoleColor.Red;
                if (string.IsNullOrEmpty(threadId))
                {
                    // Create a new thread if no ID is provided
                    persistentThread = agentsClient.Threads.CreateThread();
                    Console.WriteLine($"Created new thread, ID: {persistentThread.Id}");
                    threadId = persistentThread.Id;
                }
                else
                {
                    // Check if the provided thread ID exists
                    try
                    {

                        persistentThread = agentsClient.Threads.GetThread(threadId);
                        Console.WriteLine($"Using existing thread, ID: {persistentThread.Id}");
                    }
                    catch (RequestFailedException ex) when (ex.Status == 404)
                    {
                        Console.WriteLine($"Thread with ID {threadId} not found. Creating a new thread.");
                        persistentThread = agentsClient.Threads.CreateThread();
                        Console.WriteLine($"Created new thread, ID: {persistentThread.Id}");
                        threadId = persistentThread.Id;
                    }
                }

                string? userInput = "Hello Agent";
                while (true)
                {
                    PersistentThreadMessage messageResponse = agentsClient.Messages.CreateMessage(
                        persistentThread.Id,
                        MessageRole.User,
                        userInput);

                    ThreadRun run = agentsClient.Runs.CreateRun(
                        persistentThread.Id,
                    googleCloudExpertAgent.Id);

                    // Poll until the run reaches a terminal status
                    do
                    {
                        await Task.Delay(TimeSpan.FromMilliseconds(500));
                        run = agentsClient.Runs.GetRun(persistentThread.Id, run.Id);
                    }
                    while (run.Status == RunStatus.Queued
                        || run.Status == RunStatus.InProgress);
                    if (run.Status != RunStatus.Completed)
                    {
                        throw new InvalidOperationException($"Run failed or was canceled: {run.LastError?.Message}");
                    }

                    Pageable<PersistentThreadMessage> messages = agentsClient.Messages.GetMessages(
                        persistentThread.Id, order: ListSortOrder.Ascending);

                    // Display messages
                    foreach (PersistentThreadMessage threadMessage in messages)
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write($"{threadMessage.CreatedAt:yyyy-MM-dd HH:mm:ss} - {threadMessage.Role,10}: ");
                        foreach (MessageContent contentItem in threadMessage.ContentItems)
                        {
                            if (contentItem is MessageTextContent textItem)
                            {
                                Console.Write(textItem.Text);
                            }
                            else if (contentItem is MessageImageFileContent imageFileItem)
                            {
                                Console.Write($"<image from ID: {imageFileItem.FileId}");
                            }
                            Console.WriteLine();
                        }
                    }
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Enter your message (or type 'exit' to quit):");
                    userInput = Console.ReadLine();
                    if (userInput?.Trim().ToLower() == "exit")
                    {
                        break;
                    }
                    Console.ForegroundColor = ConsoleColor.Red;
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"There is an error /n {ex.Message}");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }
}
