# SigSpec for SignalR Core

**Experimental specification** and code generator for [SignalR Core](https://github.com/aspnet/SignalR).

Run SigSpec.Console to see a demo spec and the generated TypeScript code.

**Please let me know what you think [here](https://github.com/RSuter/SigSpec/issues/1).**

Based on [NJsonSchema](http://njsonschema.org) (see also: [NSwag](http://nswag.org)).

Original idea: https://github.com/RSuter/NSwag/issues/691

## Sample

Hub: 

```csharp
public class ChatHub : Hub<IChatClient>
{
    public Task Send(string message)
    {
        if (message == string.Empty)
        {
            return Clients.All.Welcome();
        }

        return Clients.All.Send(message);
    }

    public Task Foo(Bar bar)
    {
        return Task.CompletedTask;
    }
}

public class Bar
{
    [JsonProperty("firstName")]
    public string FirstName { get; set; }

    [JsonProperty("lastName")]
    public string LastName { get; set; }
}

public interface IChatClient
{
    Task Welcome();

    Task Send(string message);
}
```

Generated spec: 

```json
{
  "hubs": {
    "chat": {
      "name": "Chat",
      "description": "",
      "operations": {
        "Send": {
          "description": "",
          "parameters": {
            "message": {
              "type": [
                "null",
                "string"
              ],
              "description": ""
            }
          }
        },
        "Foo": {
          "description": "",
          "parameters": {
            "bar": {
              "description": "",
              "oneOf": [
                {
                  "type": "null"
                },
                {
                  "$ref": "#/definitions/Bar"
                }
              ]
            }
          }
        }
      },
      "callbacks": {
        "Welcome": {
          "description": "",
          "parameters": {}
        },
        "Send": {
          "description": "",
          "parameters": {
            "message": {
              "type": [
                "null",
                "string"
              ],
              "description": ""
            }
          }
        }
      }
    }
  },
  "definitions": {
    "Bar": {
      "type": "object",
      "additionalProperties": false,
      "properties": {
        "firstName": {
          "type": [
            "null",
            "string"
          ]
        },
        "lastName": {
          "type": [
            "null",
            "string"
          ]
        }
      }
    }
  }
}
```

Generated TypeScript code: 

```typescript
export class ChatHub {
    ChatHub(private connection: any) {
    }

    send(message: string) {
        this.connection.invoke('Send', message);
    }

    foo(bar: Bar) {
        this.connection.invoke('Foo', bar);
    }

    registerCallbacks(implementation: IChatHubCallbacks) {
        this.connection.on('Welcome', () => implementation.welcome());
        this.connection.on('Send', (message) => implementation.send(message));
    }
}

export interface IChatHubCallbacks {
    welcome();
    send(message: string);
}

export interface Bar {
    firstName: string;
    lastName: string;
}
```
