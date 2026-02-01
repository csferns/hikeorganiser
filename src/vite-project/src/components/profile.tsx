import React, { useCallback, useEffect, useMemo, useState } from 'react';
import { Box, Button, Card, CardContent, Grid, TextField, Typography } from '@mui/material';
import { useAuth } from '../auth/AuthContext.tsx';
import { apiService, Toast } from '../defaults.ts';

const Profile: React.FC = () => {
  const { user, refreshUser } = useAuth();

  const [displayName, setDisplayName] = useState<string>('');
  const [email, setEmail] = useState<string>('');
  const [userName, setUserName] = useState<string>('');

  const [currentPassword, setCurrentPassword] = useState('');
  const [newPassword, setNewPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');

  const [savingProfile, setSavingProfile] = useState(false);
  const [savingPassword, setSavingPassword] = useState(false);

  useEffect(() => {
    setDisplayName((user?.displayName as string) || (user?.name as string) || '');
    setEmail(user?.email || '');
    setUserName(user?.userName || '');
  }, [user]);

  const canSaveProfile = useMemo(() => {
    return !!email?.trim() || !!displayName?.trim() || !!userName?.trim();
  }, [email, displayName, userName]);

  const handleSaveProfile = useCallback(async () => {
    if (!canSaveProfile) return;
    try {
      setSavingProfile(true);
      await apiService.updateProfile({ email: email.trim(), userName: userName.trim(), displayName: displayName.trim() });
      await refreshUser();
      Toast.fire({ icon: 'success', title: 'Profile updated' });
    } catch (e: any) {
      Toast.fire({ icon: 'error', title: e?.response?.data?.message || 'Failed to update profile' });
    } finally {
      setSavingProfile(false);
    }
  }, [canSaveProfile, displayName, email, userName, refreshUser]);

  const handleChangePassword = useCallback(async () => {
    if (!newPassword || newPassword !== confirmPassword) {
      Toast.fire({ icon: 'warning', title: 'Passwords do not match' });
      return;
    }
    try {
      setSavingPassword(true);
      await apiService.changePassword(currentPassword, newPassword);
      setCurrentPassword('');
      setNewPassword('');
      setConfirmPassword('');
      Toast.fire({ icon: 'success', title: 'Password changed' });
    } catch (e: any) {
      Toast.fire({ icon: 'error', title: e?.response?.data?.message || 'Failed to change password' });
    } finally {
      setSavingPassword(false);
    }
  }, [currentPassword, newPassword, confirmPassword]);

  return (
    <Box>
      <Typography variant="h5" fontWeight={700} gutterBottom>
        Profile
      </Typography>

      <Grid container spacing={2} xs={12} md={6}>
        <Card>
          <CardContent>
            <Typography variant="h6" gutterBottom>Account Details</Typography>
            <Grid container spacing={2}>
              <Grid item xs={12}>
                <TextField
                  fullWidth
                  label="Display name"
                  value={displayName}
                  onChange={(e) => setDisplayName(e.target.value)}
                />
              </Grid>
              <Grid item xs={12}>
                <TextField
                  fullWidth
                  type="email"
                  label="Email"
                  value={email}
                  onChange={(e) => setEmail(e.target.value)}
                />
              </Grid>
              <Grid item xs={12}>
                <TextField
                  fullWidth
                  label="Username"
                  value={userName}
                  disabled
                />
              </Grid>
              <Grid item xs={12}>
                <Button variant="contained" onClick={handleSaveProfile} disabled={savingProfile || !canSaveProfile}>
                  {savingProfile ? 'Saving...' : 'Save changes'}
                </Button>
              </Grid>
            </Grid>
          </CardContent>
        </Card>
      </Grid>

      <Grid container xs={12} md={6}>
        <Card>
          <CardContent>
            <Typography variant="h6" gutterBottom>Change Password</Typography>
            <Grid container spacing={2}>
              <Grid item xs={12}>
                <TextField
                  fullWidth
                  label="Current password"
                  type="password"
                  value={currentPassword}
                  onChange={(e) => setCurrentPassword(e.target.value)}
                />
              </Grid>
              <Grid item xs={12}>
                <TextField
                  fullWidth
                  label="New password"
                  type="password"
                  value={newPassword}
                  onChange={(e) => setNewPassword(e.target.value)}
                />
              </Grid>
              <Grid item xs={12}>
                <TextField
                  fullWidth
                  label="Confirm new password"
                  type="password"
                  value={confirmPassword}
                  onChange={(e) => setConfirmPassword(e.target.value)}
                />
              </Grid>
              <Grid item xs={12}>
                <Button variant="outlined" onClick={handleChangePassword} disabled={savingPassword}>
                  {savingPassword ? 'Saving...' : 'Update password'}
                </Button>
              </Grid>
            </Grid>
          </CardContent>
        </Card>
      </Grid>
    </Box>
  );
};

export default Profile;
