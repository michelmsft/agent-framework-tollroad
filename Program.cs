// dotnet add package Azure.AI.OpenAI
// dotnet add package Azure.Identity
// dotnet add package Microsoft.Agents.AI.OpenAI --prerelease

using System;
using Azure.AI.OpenAI;
using Azure.Identity;
using Microsoft.Agents.AI;
using OpenAI;
using Microsoft.Extensions.AI;

AIAgent agent = new AzureOpenAIClient(
  new Uri("https://ai-xperimenthub630858472795.openai.azure.com/"),
  new AzureCliCredential())
    .GetChatClient("gpt-4o")
    .CreateAIAgent(
        name: "ToolRoadAgent",
        instructions: @"
            You are a vision assistant specialized in License Plate Recognition (LPR).
            Extract only the license plate text visible in the image.
            - Normalize characters: remove spaces; use uppercase A–Z and 0–9 when unsure.
            - If ambiguous (e.g., O/0, I/1), choose the most likely based on context.
            - Provide a confidence score in [0,1].
            - If visible, include the issuing region (state/province/country).
            - If possible, estimate a bounding box [x,y,width,height] around the plate in pixels.
            Return ONLY JSON per the provided schema.
            ");


ChatMessage message = new(ChatRole.User, [
    new TextContent("Extract the vehicle's license plate from the image."),
    new UriContent("https://media.9news.com/assets/KUSA/images/b235013f-df6b-4ef3-993c-322644e73435/20240617T233423/b235013f-df6b-4ef3-993c-322644e73435_1920x1080.jpg", "image/jpeg")
]);

Console.WriteLine(await agent.RunAsync(message));