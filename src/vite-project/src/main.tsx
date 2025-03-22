import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import { BrowserRouter, Route, Routes } from "react-router";
import { FluentProvider, webLightTheme } from '@fluentui/react-components';
import App from './App.tsx'
import Schedule from "./components/schedule.tsx";

createRoot(document.getElementById('root')!).render(
  <StrictMode>
      <FluentProvider theme={webLightTheme}>
          <BrowserRouter>
              <Routes>
                  <Route path="/" element={<App />} />
                  <Route path="/schedule" element={<Schedule />} />
              </Routes>
          </BrowserRouter>
      </FluentProvider>
  </StrictMode>
)
