import Vue from 'vue';
declare module 'vue/types/vue' {
  interface SigSpecUiSettings {
    route: string;
    baseUrl: string;
  }
  interface Vue {
    $settings: SigSpecUiSettings;
  }
}
