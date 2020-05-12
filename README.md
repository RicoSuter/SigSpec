# SigSpec for SignalR Core

**Experimental API endpoint specification** and code generator for [SignalR Core](https://github.com/aspnet/SignalR).

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

    public Task AddPerson(Person person)
    {
        Clients.Others.PersonAdded(person);
        return Task.CompletedTask;
    }

    public ChannelReader<Event> GetEvents()
    {
        var channel = Channel.CreateUnbounded<Event>();
        // TODO: Write events
        return channel.Reader;
    }
}

public class Event
{
    public string Type { get; set; }
}

public class Person
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

    Task PersonAdded(Person person);
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
              "type": "string",
              "description": ""
            }
          }
        },
        "AddPerson": {
          "description": "",
          "parameters": {
            "person": {
              "description": "",
              "oneOf": [
                {
                  "$ref": "#/definitions/Person"
                }
              ]
            }
          }
        },
        "GetEvents": {
          "description": "",
          "parameters": {},
          "returntype": {
            "description": "Provides a base class for reading from a channel.",
            "oneOf": [
              {
                "$ref": "#/definitions/Event"
              }
            ]
          },
          "type": "Observable"
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
              "type": "string",
              "description": ""
            }
          }
        },
        "PersonAdded": {
          "description": "",
          "parameters": {
            "person": {
              "description": "",
              "oneOf": [
                {
                  "$ref": "#/definitions/Person"
                }
              ]
            }
          }
        }
      }
    }
  },
  "definitions": {
    "Person": {
      "type": "object",
      "additionalProperties": false,
      "properties": {
        "firstName": {
          "type": ["null", "string"]
        },
        "lastName": {
          "type": ["null", "string"]
        }
      }
    },
    "Event": {
      "type": "object",
      "additionalProperties": false,
      "properties": {
        "type": {
          "type": ["null", "string"]
        }
      }
    }
  }
}
```

Generated TypeScript code:

```typescript
import { HubConnection, IStreamResult } from '@microsoft/signalr';

export class ChatHub {
  constructor(private connection: HubConnection) {}

  send(message: string): Promise<void> {
    return this.connection.invoke('Send', message);
  }

  addPerson(person: Person): Promise<void> {
    return this.connection.invoke('AddPerson', person);
  }

  getEvents(): IStreamResult<Event> {
    return this.connection.stream('GetEvents');
  }

  onWelcome(func: () => void): void {
    this.connection.on('Welcome', func);
  }

  unregisterWelcome(func: () => void): void {
    this.connection.off('Welcome', func);
  }
  onSend(func: (message: string) => void): void {
    this.connection.on('Send', func);
  }

  unregisterSend(func: (message: string) => void): void {
    this.connection.off('Send', func);
  }
  onPersonAdded(func: (person: Person) => void): void {
    this.connection.on('PersonAdded', func);
  }

  unregisterPersonAdded(func: (person: Person) => void): void {
    this.connection.off('PersonAdded', func);
  }

  registerCallbacks(implementation: IChatHubCallbacks) {
    this.connection.on('Welcome', implementation.welcome);
    this.connection.on('Send', implementation.send);
    this.connection.on('PersonAdded', implementation.personAdded);
  }

  unregisterCallbacks(implementation: IChatHubCallbacks) {
    this.connection.off('Welcome', implementation.welcome);
    this.connection.off('Send', implementation.send);
    this.connection.off('PersonAdded', implementation.personAdded);
  }
}

export interface IChatHubCallbacks {
  welcome(): void;
  send(message: string): void;
  personAdded(person: Person): void;
}

export interface Person {
  firstName: string | null;
  lastName: string | null;
}

export interface Event {
  type: string | null;
}
```
