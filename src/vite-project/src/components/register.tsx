import { useState, FormEvent } from 'react';
import {
    TextField,
    Button,
    Stack,
    Typography,
    Container,
    Paper,
    Box,
    Avatar,
    Alert,
    Link as MuiLink
} from '@mui/material'
import PersonAddAlt1OutlinedIcon from '@mui/icons-material/PersonAddAlt1Outlined';
import { apiService } from '../defaults.ts';
import { useLocation, useNavigate, Link as RouterLink } from 'react-router';
import { useAuth } from '../auth/AuthContext.tsx';

export default function Register() {
    const [email, setEmail] = useState<string>("");
    const [password, setPassword] = useState<string>("");
    const [confirmPassword, setConfirmPassword] = useState<string>("");
    const [referral, setReferral] = useState<string | undefined>(undefined);
    const [error, setError] = useState<string | null>(null);
    const [submitting, setSubmitting] = useState<boolean>(false);

    const navigate = useNavigate();
    const location = useLocation() as any;
    const redirectTo = location.state?.from?.pathname || "/";
    const { refreshUser } = useAuth();

    const handleSubmit = async (e: FormEvent) : Promise<void> => {
        e.preventDefault();
        setError(null);

        if (password !== confirmPassword) {
            setError('Passwords do not match');
            return;
        }
        setSubmitting(true);
        try {
            await apiService.register(email.trim(), password, referral?.trim() || undefined);
            await refreshUser();
            navigate(redirectTo, { replace: true });
        } catch (ex: any) {
            setError(ex?.response?.data?.title || ex?.message || 'Unable to register');
        } finally {
            setSubmitting(false);
        }
    }

    return (
        <Box className="login-bg" sx={{ minHeight: '100dvh', display: 'grid', placeItems: 'center', p: 2 }}>
            <Container maxWidth="xs">
                <Paper elevation={8} className="login-card" sx={{ p: 3, borderRadius: 3, backdropFilter: 'blur(6px)' }}>
                    <Stack spacing={2} alignItems="center" sx={{ mb: 1 }}>
                        <Avatar sx={{ bgcolor: 'primary.main' }}>
                            <PersonAddAlt1OutlinedIcon />
                        </Avatar>
                        <Typography variant="h5" component="h1" fontWeight={600}>
                            Create account
                        </Typography>
                        <Typography variant="body2" color="text.secondary" align="center">
                            Join HikeOrganiser to start planning your adventures.
                        </Typography>
                    </Stack>

                    {error && (
                        <Alert severity="error" sx={{ mb: 2 }}>
                            {error}
                        </Alert>
                    )}

                    <Box component="form" onSubmit={handleSubmit} noValidate>
                        <Stack spacing={2}>
                            <TextField
                                label="Email"
                                type="email"
                                value={email}
                                onChange={(e) => setEmail(e.target.value)}
                                autoComplete="email"
                                required
                                fullWidth
                            />
                            <TextField
                                label="Password"
                                type="password"
                                value={password}
                                onChange={(e) => setPassword(e.target.value)}
                                autoComplete="new-password"
                                required
                                fullWidth
                            />
                            <TextField
                                label="Confirm password"
                                type="password"
                                value={confirmPassword}
                                onChange={(e) => setConfirmPassword(e.target.value)}
                                autoComplete="new-password"
                                required
                                fullWidth
                            />
                            <TextField
                                label="Referral code (optional)"
                                value={referral ?? ''}
                                onChange={(e) => setReferral(e.target.value)}
                                fullWidth
                            />
                            <Button
                                disabled={submitting}
                                variant="contained"
                                color="primary"
                                type="submit"
                                size="large"
                                fullWidth
                            >
                                {submitting ? 'Creating account…' : 'Create account'}
                            </Button>
                        </Stack>
                    </Box>

                    <Stack direction="row" justifyContent="space-between" sx={{ mt: 2 }}>
                        <Typography variant="body2" color="text.secondary">
                            Already have an account?
                        </Typography>
                        <MuiLink component={RouterLink} to="/login">
                            Sign in
                        </MuiLink>
                    </Stack>
                </Paper>
                <Typography variant="caption" color="text.secondary" display="block" align="center" sx={{ mt: 2 }}>
                    HikeOrganiser
                </Typography>
            </Container>
        </Box>
    )
}