# Scriptable Web Request

## Overview
**Scriptable Web Request** is a Unity tool that simplifies the process of making HTTP requests (GET, POST, PUT, DELETE, etc.) using ScriptableObjects. This allows you to handle web requests in a modular and reusable way, making your code cleaner and easier to maintain.

## Features
- Modular HTTP request handling with ScriptableObjects.
- Support for GET, POST, PUT, and DELETE methods.
- Easy integration with Unity projects.
- Customizable headers and content type.
- Asynchronous request handling.
- Debugging support to monitor requests and responses.
- Support Cancellation Token for cancelling the on-progress request.

## Installation

### Using Unity Package Manager
Because unity doesn't support custom dependency for package outside Unity Registry, 
you need to install [Unity-Web-Request](https://github.com/jeffreylanters/unity-web-requests) library from [Jeffrey Lanters](https://github.com/jeffreylanters).

You could install both of the packages by add the following line to the `dependencies` section of your project's `manifest.json` file
```json
"nl.jeffreylanters.web-requests": "git+https://github.com/jeffreylanters/unity-web-requests"
"com.nabilahkishou.scriptable-web-request": "https://github.com/NabilahKishou/Scriptable-Web-Request.git"
```

## Usage

### Setting Up a Scriptable Web Request
1. Create a new ScriptableObject by right-clicking in the **Project** window and navigating to **Create > Web Request > Basic**.
2. Configure the request by setting the following properties:
   - **Endpoint**: The endpoint you want to call.
   - **HTTP Method**: Choose between GET, POST, PATCH, PUT, or DELETE.
   - **Content Type**: Define the data type you want to send, mostly using Application JSON.
   - **Need Auth**: If the request needed an auth you can check this.

### Making a Request
Use the `ScriptableWebRequest` object in your scripts to send requests. For example:
```csharp
BasicRequest _pingRequest; // request for ping to server

async Task<bool> Ping()
{
   WebRequestResponse response = null;
   try {
       response = await ping.SendRequest(); // sending the request
   }
   catch (WebRequestException e) {
       return false;
   }
   
   return response.Text().Contains("pong");
}
```

## License
This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

## Contact
For questions or support, please open an issue in the repository or contact [NabilahKishou](https://github.com/NabilahKishou).

## Thank-You Notes

This library could exist solely because the [library](https://github.com/jeffreylanters/unity-web-requests) from [Jeffrey Lanters](https://github.com/jeffreylanters),
kindly checkout his repository yourself.