<template>
  <div class="opblock opblock-get" :class="{ 'is-open': open }" id="operations-pet-uploadFile">
    <div @click="open = !open" class="opblock-summary opblock-summary-get">
      <span class="opblock-summary-method">Callback</span>
      <span class="opblock-summary-path" data-path="/pet/{petId}">
        <a class="nostyle" href="#/pet/getPetById">
          <span>{{ callback.name }}</span>
        </a>
      </span>
      <div class="opblock-summary-description">{{ callback.description }}</div>
      <button class="authorization__btn unlocked" aria-label="authorization button unlocked">
        <svg width="20" height="20">
          <use href="#unlocked" xlink:href="#unlocked" />
        </svg>
      </button>
    </div>
    <div v-if="open" style="height: auto; border: none; margin: 0px; padding: 0px;">
      <div class="opblock-body">
        <div class="opblock-description-wrapper">
          <div class="opblock-description">
            <div class="markdown">
              <p>{{callback.description}}</p>
            </div>
          </div>
        </div>
        <div class="opblock-section">
          <div class="opblock-section-header">
            <div class="tab-header">
              <h4 class="opblock-title">Parameters</h4>
            </div>
            <div class="try-out">
              <button class="btn success__btn" @click="startListening" v-if="!listening">Start Listening</button>
              <button class="btn danger__btn" @click="stopListening" v-else>Stop Listening</button>
            </div>
          </div>
          <div class="parameters-container" v-if="callback.parameters.length > 0">
            <div class="table-container">
              <table class="parameters">
                <thead>
                  <tr>
                    <th>Name</th>
                    <th>Type</th>
                  </tr>
                </thead>
                <tbody>
                  <tr :key="param.name" v-for="param in callback.parameters">
                    <td>{{ param.name }}</td>
                    <td>
                      {{ param.description }}
                      <div v-if="param.type">{{ param.type }}</div>
                      <div v-else>
                        {{ param.oneOf[0].$ref }} -
                        <pre> {{ definition(param.oneOf[0].$ref) }}</pre>
                      </div>
                    </td>
                  </tr>
                </tbody>
              </table>
            </div>
          </div>
        </div>
        <div class="execute-wrapper"></div>
        <div class="responses-wrapper">
          <div class="opblock-section-header">
            <h4>Responses</h4>
            <label>
              <span>Response content type</span>
            </label>
          </div>
          <div class="responses-inner"></div>
          <pre class="microlight">
                <div :key="index" v-for="(message, index) in callbackMessages">{{message}}</div>
          </pre>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import Vue from 'vue';
import { Callback, Definition } from '../models/Spec';
import { HubConnection } from '@microsoft/signalr';

export default Vue.extend({
    props: {
        callback: {
            type: Object as () => Callback,
            required: true
        },
        definitions: {
            type: Array as () => Definition[],
            required: true
        },
        connection: {
            type: Object as () => HubConnection,
            required: true
        }
    },
    data() {
        return {
            open: false,
            callbackMessages: [] as {
                timeStamp: Date;
                params: Record<string, any>[];
            }[],
            listening: false
        };
    },
    created() {
        this.startListening();
    },
    methods: {
        definition(path: string): Definition | undefined {
            const parts = path.split('/');
            const name = parts[parts.length - 1];
            return this.definitions.find(d => d.name == name);
        },
        listenerHandler(...args: Record<string, any>[]) {
            this.callbackMessages.push({
                timeStamp: new Date(Date.now()),
                params: this.callback.parameters.map((p, i) => ({
                    name: p.name,
                    value: args[i]
                }))
            });
        },
        startListening() {
            this.listening = true;
            this.connection.on(this.callback.name, this.listenerHandler);
        },
        stopListening() {
            this.listening = false;
            this.connection.off(this.callback.name, this.listenerHandler);
        }
    }
});
</script>

<style>
.swagger-ui.sigspec-ui .opblock-body pre.microlight {
    width: 80%;
    margin-left: 10%;
    margin-bottom: 20px;
}
</style>