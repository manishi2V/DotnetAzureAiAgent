---

<p align="center">
  <img src="https://avatars.githubusercontent.com/u/165612349?s=200&v=4" width="100" alt="Azure AI Logo" />
  <h1 align="center">DotnetAzureAiAgent ☁️🤖</h1>
  <p align="center">Interactive Console Chatbot for Google Cloud Queries using .NET 9 + Azure AI</p>
</p>

<p align="center">
  <img alt=".NET Version" src="https://img.shields.io/badge/.NET-9.0-blue?logo=dotnet" />
  <img alt="License" src="https://img.shields.io/github/license/manishi2V/DotnetAzureAiAgent" />
  <img alt="Azure AI" src="https://img.shields.io/badge/Azure%20AI-Expert%20Agent-green?logo=microsoftazure" />
</p>

---

## ✅ Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- Azure Cloud Account
- Azure AI Foundry project with a Google Cloud Expert Agent

---

## 🔐 Set Environment Variables

**Windows:**
```sh
setx AZURE_OPENAI_API_ENDPOINT "https://<your-endpoint>.openai.azure.com/"
setx GOOGLE_CLOUD_EXPERT_AGENT_ID "<your-agent-id>"
```
**Linux/macOS:**
```sh
export AZURE_OPENAI_API_ENDPOINT="https://<your-endpoint>.openai.azure.com/"
export GOOGLE_CLOUD_EXPERT_AGENT_ID="<your-agent-id>"
```
> 💡 Restart your terminal or IDE after adding variables.

---

## 🚀 Clone and Run

```bash
git clone https://github.com/manishi2V/DotnetAzureAiAgent.git
cd DotnetAzureAiAgent
dotnet build
dotnet run
```

---

## 💬 How It Works

- Connects to your Azure OpenAI endpoint and Google Cloud Expert Agent.
- Lists your previous conversation threads.
- Choose an existing thread or start a new one.
- Ask any Google Cloud-related question!
- Conversation continues until you type `exit`.

---

## 🧠 Example Session

```bash
Available threads:
Thread ID: 12345, Created on: 2025-07-01 10:10:00
Enter the thread ID you want to use (or press Enter to create a new one):

Enter your message (or type 'exit' to quit):
> How do I set up a VM on Google Cloud?
🤖 Agent: To set up a VM on Google Cloud, go to Compute Engine, click "Create Instance", and follow the prompts...

> exit
```

---

## ⚙️ Customization

- Modify agent settings or endpoints in your environment variables.
- Extend or change the agent logic in `GoogleCloudExpertAgent.cs`.

---

## 📄 License

This project is licensed under the **MIT License**.  
See the [LICENSE](./LICENSE) file for details.

---

## 🙌 Credits

- Built with ❤️ using [.NET 9](https://dotnet.microsoft.com/)
- Powered by [Azure AI Foundry](https://azure.microsoft.com/en-us/products/machine-learning/)

---

> ⭐ If you found this helpful, give it a star on [GitHub](https://github.com/manishi2V/DotnetAzureAiAgent)!

---
