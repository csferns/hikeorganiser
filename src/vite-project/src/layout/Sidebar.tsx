import React from 'react';
import { NavLink, useLocation } from 'react-router';
import {
  Avatar,
  Box,
  Divider,
  Drawer,
  List,
  ListItem,
  ListItemButton,
  ListItemIcon,
  ListItemText,
  Stack,
  Toolbar,
  Typography,
  useTheme
} from '@mui/material';
import EventIcon from '@mui/icons-material/Event';
import CalendarMonthIcon from '@mui/icons-material/CalendarMonth';
import ListAltIcon from '@mui/icons-material/ListAlt';
import LogoutIcon from '@mui/icons-material/Logout';
import ManageAccountsIcon from '@mui/icons-material/ManageAccounts';
import { useAuth } from '../auth/AuthContext.tsx';

const drawerWidth = 300;

interface SidebarProps {
  mobileOpen: boolean;
  onClose: () => void;
}

const Sidebar: React.FC<SidebarProps> = ({ mobileOpen, onClose }) => {
  const theme = useTheme();
  const { user, logout } = useAuth();
  const location = useLocation();

  const navItems = [
    { to: '/', label: 'Events', icon: <EventIcon /> },
    { to: '/schedule', label: 'Schedule', icon: <CalendarMonthIcon /> },
    { to: '/bucketlist', label: 'Bucket List', icon: <ListAltIcon /> },
  ];

  const content = (
    <Box sx={{ height: '100%', display: 'flex', flexDirection: 'column' }}>
      <Toolbar>
        <Typography variant="h6" noWrap component="div" fontWeight={700}>
          Hike Organiser
        </Typography>
      </Toolbar>
      <Divider />
      <List>
        {navItems.map((item) => {
          const selected = location.pathname === item.to;
          return (
            <ListItem key={item.to} disablePadding>
              <ListItemButton
                component={NavLink as any}
                to={item.to}
                selected={selected}
                onClick={onClose}
              >
                <ListItemIcon>
                  {item.icon}
                </ListItemIcon>
                <ListItemText primary={item.label} />
              </ListItemButton>
            </ListItem>
          );
        })}
      </List>

      <Box sx={{ mt: 'auto' }}>
        <Divider />
        <Stack direction="row" alignItems="center" spacing={1.5} sx={{ p: 2, pt: 2.5 }}>
          <Avatar sx={{ bgcolor: theme.palette.primary.main }}>
            {(user?.displayName || user?.email || '?').slice(0, 1).toUpperCase()}
          </Avatar>
          <Box sx={{ minWidth: 0 }}>
            <Typography variant="body2" fontWeight={600} noWrap>
              {user?.displayName || 'User'}
            </Typography>
            <Typography variant="caption" color="text.secondary" noWrap>
              {user?.email}
            </Typography>
          </Box>
          <Box sx={{ flexGrow: 1 }} />
          <ListItemButton component={NavLink as any} to="/profile" sx={{ width: 44, borderRadius: 1 }} title="Profile" onClick={onClose}>
            <ListItemIcon sx={{ minWidth: 0 }}>
              <ManageAccountsIcon fontSize="small" />
            </ListItemIcon>
          </ListItemButton>
          <ListItemButton onClick={logout} sx={{ width: 44, borderRadius: 1 }} title="Sign out">
            <ListItemIcon sx={{ minWidth: 0 }}>
              <LogoutIcon fontSize="small" />
            </ListItemIcon>
          </ListItemButton>
        </Stack>
      </Box>
    </Box>
  );

  return (
    <Box component="nav" sx={{ width: { sm: drawerWidth }, flexShrink: { sm: 0 } }} aria-label="navigation sidebar">
      <Drawer
        variant="temporary"
        open={mobileOpen}
        onClose={onClose}
        ModalProps={{ keepMounted: true }}
        sx={{
          display: { xs: 'block', sm: 'none' },
          '& .MuiDrawer-paper': { boxSizing: 'border-box', width: drawerWidth }
        }}
      >
        {content}
      </Drawer>
      <Drawer
        variant="permanent"
        sx={{
          display: { xs: 'none', sm: 'block' },
          '& .MuiDrawer-paper': { boxSizing: 'border-box', width: drawerWidth }
        }}
        open
      >
        {content}
      </Drawer>
    </Box>
  );
};

export default Sidebar;
export { drawerWidth };
