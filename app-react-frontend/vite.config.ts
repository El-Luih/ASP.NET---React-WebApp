import react from '@vitejs/plugin-react'
import { defineConfig } from 'vite'

// https://vite.dev/config/
export default defineConfig({
  plugins: [react()],
  server: {
    proxy: {
      '/WeatherForecast': {
        target: 'http://localhost:5069',
        changeOrigin: true,
        secure: false,
      },
    },
  },
})
