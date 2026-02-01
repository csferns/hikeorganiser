import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import { BrowserRouter, Route, Routes } from "react-router";
import { LocalizationProvider } from '@mui/x-date-pickers';
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs';
import { ThemeProvider, createTheme } from '@mui/material/styles';
import CssBaseline from '@mui/material/CssBaseline';
import './index.css'
import Schedule from "./components/schedule.tsx";
import Events from "./components/events.tsx";
import BucketList from "./components/bucketlist.tsx";
import Login from "./components/login.tsx";
import Register from "./components/register.tsx";
import Profile from "./components/profile.tsx";
import { AuthProvider } from './auth/AuthContext.tsx';
import ProtectedRoute from './auth/ProtectedRoute.tsx';
import AppLayout from './layout/AppLayout.tsx';

const darkTheme = createTheme({
    palette: {
        mode: 'dark',
    },
});


createRoot(document.getElementById('root')!).render(
  <StrictMode>
      <ThemeProvider theme={darkTheme}>
          <CssBaseline />
          <LocalizationProvider dateAdapter={AdapterDayjs}>
              <BrowserRouter>
                  <AuthProvider>
                      <Routes>
                          <Route path="/login" element={<Login />} />
                          <Route path="/register" element={<Register />} />
                          <Route path="/" element={<ProtectedRoute><AppLayout><Events /></AppLayout></ProtectedRoute>} />
                          <Route path="/schedule" element={<ProtectedRoute><AppLayout><Schedule /></AppLayout></ProtectedRoute>} />
                          <Route path="/bucketlist" element={<ProtectedRoute><AppLayout><BucketList /></AppLayout></ProtectedRoute>} />
                          <Route path="/profile" element={<ProtectedRoute><AppLayout><Profile /></AppLayout></ProtectedRoute>} />
                      </Routes>
                  </AuthProvider>
              </BrowserRouter>
          </LocalizationProvider>          
      </ThemeProvider>
  </StrictMode>,
)
