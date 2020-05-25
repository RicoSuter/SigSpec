<template>
  <div class="opblock opblock-post" :class="{ 'is-open': open }">
    <div class="opblock-summary opblock-summary-post" @click="open = !open">
      <span class="opblock-summary-method">Operation</span>
      <span class="opblock-summary-path">
        <span>{{ operation.name }}</span>
      </span>
      <div class="opblock-summary-description">{{ operation.description }}</div>
      <button class="authorization__btn unlocked" aria-label="authorization button unlocked">
        <svg width="20" height="20">
          <use href="#unlocked" xlink:href="#unlocked" />
        </svg>
      </button>
      <!-- react-empty: 129 -->
    </div>
    <div v-if="open">
      <div class="opblock-body">
        <div class="opblock-section">
          <div class="opblock-section-header">
            <div class="tab-header">
              <h4 class="opblock-title">Parameters</h4>
            </div>
            <div class="try-out">
              <button class="btn try-out__btn">Try it out</button>
            </div>
          </div>
          <div class="parameters-container">
            <div class="table-container">
              <table class="parameters">
                <thead>
                  <tr>
                    <th class="col_header parameters-col_name">Name</th>
                    <th class="col_header parameters-col_description">Description</th>
                  </tr>
                </thead>
                <tbody>
                  <tr :key="param.name" v-for="param in operation.parameters">
                    <td class="parameters-col_name">
                      <div class="parameter__name required">
                        {{ param.name }}
                        <span style="color: red;">&nbsp;*</span>
                      </div>
                      <div class="parameter__type"></div>
                      <div class="parameter__deprecated"></div>
                      <div class="parameter__in">( path )</div>
                    </td>
                    <td class="parameters-col_description">
                      {{ param.description }}
                      <div v-if="param.type">
                        <input type="text" v-model="param.value" />
                        {{ param.type }}
                      </div>
                      <div v-else-if="param.oneOf">
                        <definition-editor v-model="param.value" :definition="definition(param.oneOf[0].$ref)" />
                      </div>
                    </td>
                  </tr>
                </tbody>
              </table>
            </div>
          </div>
        </div>
        <div class="execute-wrapper">
          <button @click="send" class="btn execute opblock-control__btn">Execute</button>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import Vue from 'vue';
import DefinitionEditor from '@/components/DefinitionEditor.vue';
import { Operation, Definition } from '@/models/Spec';
import { HubConnection } from '@microsoft/signalr';
export default Vue.extend({
  components: {
    DefinitionEditor
  },
  props: {
    operation: {
      type: Object as () => Operation
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
      open: false
    };
  },
  methods: {
    definition(path: string): Definition | undefined {
      const parts = path.split('/');
      const name = parts[parts.length - 1];
      return this.definitions.find(d => d.name == name);
    },
    send() {
      const params = this.operation.parameters.map(p => {
        if (p.type) {
          return p.value;
        } else if (p.oneOf) {
          return JSON.parse(p.value);
        }
      });
      this.connection.invoke(this.operation.name, ...params);
    }
  }
});
</script>

<style scoped></style>
