# SilentSender

**SilentSender** is a lightweight API for anonymous feedback, allowing users to send messages with meaningful tags like Praise, Suggestion, or Criticism.

---

## Features

-   Submit anonymous messages with a tag (`Praise`, `Suggestion`, or `Criticism`)

---

## Getting Started

### Requirements

-   .NET 9 SDK
-   SQL Server or any EF Core-supported database

### Installation

1. Clone the repository:

    ```bash
    git clone https://github.com/yourusername/silentsender.git
    cd silentsender

    ```

2. Configure your database in appsettings.json.

3. Apply EF Core migrations:

    ```bash
    dotnet ef database update

    ```

4. Run the project:

    ```bash
    dotnet run
    ```

## API Usage

### Submit a message

```bash
POST /api/messages
Content-Type: application/json

{
  "content": "The team did a great job!",
  "sender": null,
  "tag": "Praise"
}
```

### Sample Response

```bash
{
  "success": true,
  "message": "Message submitted successfully."
}
```
