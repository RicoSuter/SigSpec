function parameterBuilder(parameters: Record<string, any>) {
  const outParams: Parameter[] = [];
  for (const key in parameters) {
    const element = parameters[key];
    outParams.push({
      name: key,
      description: element.description,
      type: element.type,
      oneOf: element.oneOf,
      value: null,
    });
  }
  return outParams;
}

function callBackBuilder(callbacks: Record<string, any>) {
  const outCallbacks: Callback[] = [];
  for (const key in callbacks) {
    const element = callbacks[key];
    outCallbacks.push({
      name: key,
      description: element.description,
      parameters: parameterBuilder(element.parameters),
    });
  }
  return outCallbacks;
}
function operationBuilder(operations: Record<string, any>) {
  const outOperations: Callback[] = [];
  for (const key in operations) {
    const element = operations[key];
    outOperations.push({
      name: key,
      description: element.description,
      parameters: parameterBuilder(element.parameters),
    });
  }
  return outOperations;
}

function propertyBuilder(properties: Record<string, any>) {
  const outProperties: Property[] = [];
  for (const key in properties) {
    const element = properties[key];
    outProperties.push({
      name: key,
      types: element.type,
    });
  }
  return outProperties;
}

export function specBuilder(spec: Record<string, any>): Spec {
  const createdSpec: Spec = {
    hubs: [],
    definitions: [],
  };
  for (const key in spec.hubs) {
    const hub = spec.hubs[key];
    createdSpec.hubs.push({
      name: key,
      description: hub.description,
      callbacks: callBackBuilder(hub.callbacks),
      operations: operationBuilder(hub.operations),
    });
  }
  for (const key in spec.definitions) {
    const def = spec.definitions[key];
    createdSpec.definitions.push({
      name: key,
      description: def.description,
      properties: propertyBuilder(def.properties),
    });
  }

  return createdSpec;
}

export interface Spec {
  hubs: Hub[];
  definitions: Definition[];
}

export interface Hub {
  name: string;
  description: string;
  operations: Operation[];
  callbacks: Callback[];
}

export interface Operation {
  name: string;
  description: string;
  parameters: Parameter[];
}

export interface Parameter {
  name: string;
  description: string;
  type: string | Record<string, any> | null;
  oneOf: {
    $ref: string;
  };
  value: any;
}

export interface Callback {
  name: string;
  description: string;
  parameters: Parameter[];
}

export interface Definition {
  name: string;
  description: string;
  properties: Property[];
}

export interface Property {
  name: string;
  types: string[];
}
