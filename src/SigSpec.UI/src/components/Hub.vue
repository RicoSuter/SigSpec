<template>
  <div class="opblock-tag-section is-open">
    <h4 @click="open = !open" class="opblock-tag" id="operations-tag-pet" data-tag="pet" data-is-open="true">
      <a class="nostyle" href="#/pet">
        <span>{{ hub.name }}</span>
      </a>
      <small>
        <div class="markdown">
          <p>{{ hub.description }}</p>
        </div>
      </small>
      <div>
        <small class="btn success__btn">{{connectionStatus}}</small>
      </div>
      <button class="expand-operation" title="Collapse operation">
        <svg class="arrow" width="20" height="20">
          <use href="#large-arrow-down" xlink:href="#large-arrow-down" />
        </svg>
      </button>
    </h4>
    <div v-if="open">
      <operation :key="operation.name" :connection="connection" v-for="operation in hub.operations" :operation="operation" :name="operation.name" :definitions="definitions" />

      <callback :key="callback.name" :connection="connection" v-for="callback in hub.callbacks" :callback="callback" :definitions="definitions" :name="callback.name" />
    </div>
  </div>
</template>

<script lang="ts">
import Vue from 'vue';
import { Hub, Definition } from '../models/Spec';
import Operation from '@/components/Operation.vue';
import Callback from '@/components/Callback.vue';
import {
    HubConnectionBuilder,
    HubConnection,
    HubConnectionState
} from '@microsoft/signalr';

export default Vue.extend({
    components: {
        Operation,
        Callback
    },
    data() {
        return {
            open: true,
            connection: {} as HubConnection
        };
    },
    created() {
        this.connect();
    },
    props: {
        hub: {
            type: Object as () => Hub,
            required: true
        },
        definitions: {
            type: Array as () => Definition[],
            required: true
        },
        baseURL: {
            type: String,
            required: true
        }
    },
    methods: {
        connect() {
            console.log(this.baseURL);
            this.connection = new HubConnectionBuilder()
                .withUrl(`${this.baseURL}${this.hub.name.replace('hub', '')}`)
                .withAutomaticReconnect()
                .build();

            this.connection.start();
        }
    },
    computed: {
        connectionStatus(): string {
            const state = this.connection.state;
            if (state == HubConnectionState.Connected) {
                return 'connected';
            }
            if (
                state == HubConnectionState.Reconnecting ||
                state == HubConnectionState.Connecting
            ) {
                return 'retrying';
            }

            return 'failed';
        }
    }
});
</script>

<style scoped>
</style>