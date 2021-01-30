import Vue from 'vue';
import App from './App.vue';

import 'swagger-ui/dist/swagger-ui.css';
import { SigSpecUiSettings } from 'vue/types/vue';

Vue.config.productionTip = false;

declare global {
  interface Window {
    baseURL: string;
    sigSpecUiSettings: SigSpecUiSettings | null;
  }
}

const defaultSettings: SigSpecUiSettings = { baseUrl: 'https://localhost:5001', route: '/sigspec' };
const settings = window.sigSpecUiSettings != null ? window.sigSpecUiSettings : defaultSettings;

Vue.prototype.$settings = settings;

new Vue({
  render: h => h(App)
}).$mount('#app');
