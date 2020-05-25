<template>
  <div id="app" class="swagger-ui sigspec-ui">
    <div class="topbar">
      <div class="wrapper">
        <div class="topbar-wrapper">
          <a rel="noopener noreferrer" class="link">
            <h4 class="title">SigSpec UI</h4>
          </a>
          <div class="download-url-wrapper">
            <input type="text" class="download-url-input" v-model="specUrl" style />
            <button @click="loadSpec" class="download-url-button button">Explore</button>
          </div>
        </div>
      </div>
    </div>
    <div class="loading-container" v-if="loading">
      <div class="info">
        <div class="loading-container">
          <!--      <h4 class="title">Failed to load API definition.</h4> -->
          <div class="loading"></div>
        </div>
      </div>
    </div>
    <div class="wrapper">
      <section class="block col-12 block-desktop col-12-desktop">
        <hub :key="hub.name" v-for="hub in spec.hubs" :hub="hub" :definitions="spec.definitions" />

        <section class="models is-open">
          <h4>
            <span>Models</span>
            <svg width="20" height="20">
              <use xlink:href="#large-arrow-down" />
            </svg>
          </h4>
          <definition :key="definition.name" v-for="definition in spec.definitions" :definition="definition" />
        </section>
      </section>
    </div>
  </div>
</template>

<script lang="ts">
import Vue from 'vue';
import Hub from '@/components/Hub.vue';
import Definition from '@/components/Definition.vue';
import { Spec, specBuilder } from './models/Spec';

export default Vue.extend({
  name: 'App',
  components: {
    Hub,
    Definition
  },
  data() {
    return {
      specDef: null as Record<string, any> | null,
      loading: false
    };
  },
  computed: {
    spec(): Spec {
      if (this.specDef) {
        return specBuilder(this.specDef);
      }
      return { hubs: [], definitions: [] };
    },
    specUrl(): string {
      return this.$settings.baseUrl + `${this.$settings.route}/v1/sigspec.json`;
    }
  },
  created() {
    this.loadSpec();
  },
  methods: {
    loadSpec() {
      this.loading = true;
      this.specDef = null;
      fetch(this.specUrl, { mode: 'cors' })
        .then(response => {
          return response.text();
        })
        .then(text => {
          this.specDef = JSON.parse(text);
        })
        .catch(error => {
          alert('Request failed' + error);
        })
        .finally(() => {
          this.loading = false;
        });
    },
    definition(path: string) {
      const parts = path.split('/');
      const name = parts[parts.length - 1];
      return (this.spec.definitions as any)[name];
    },
    propertyType(prop: string[]) {
      const type = prop[prop.length - 1];
      const nullable = prop[0] == 'null';

      return `${type}${nullable ? '!' : ''}`;
    }
  }
});
</script>

<style>
body {
  margin: 0;
}

.title {
  margin: 0;
}
.swagger-ui .btn.success__btn {
  color: #49cc90;
  border-color: #49cc90;
}
.swagger-ui .btn.danger__btn {
  border-color: #ff6060;
  color: #ff6060;
}
</style>
