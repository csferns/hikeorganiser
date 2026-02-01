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
    Link as MuiLink,
    FormControlLabel,
    Checkbox
} from '@mui/material'
import LockOutlinedIcon from '@mui/icons-material/LockOutlined';
import { useAuth } from '../auth/AuthContext.tsx';
import { useLocation, useNavigate, Link as RouterLink } from 'react-router';

export default function Login() {
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [error, setError] = useState<string | null>(null);
    const [submitting, setSubmitting] = useState<boolean>(false);
    const [remember, setRemember] = useState<boolean>(true);
    const { login } = useAuth();
    const navigate = useNavigate();
    const location = useLocation() as any;
    const redirectTo = location.state?.from?.pathname || "/";
    
    const handleSubmit = async (e: FormEvent) : Promise<void> => {
        e.preventDefault()
        setError(null);
        setSubmitting(true);
        try {
            // Remember me is currently cosmetic until backend supports persistent cookie flags.
            await login(email.trim(), password);
            navigate(redirectTo, { replace: true });
        } catch (ex : any) {
            setError(ex?.response?.data?.title || ex?.message || 'Unable to login');
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
                            <LockOutlinedIcon />
                        </Avatar>
                        <Typography variant="h5" component="h1" fontWeight={600}>
                            Sign in
                        </Typography>
                        <Typography variant="body2" color="text.secondary" align="center">
                            Welcome back. Please enter your credentials to continue.
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
                                autoComplete="current-password"
                                required
                                fullWidth
                            />
                            <FormControlLabel
                                control={<Checkbox checked={remember} onChange={(e) => setRemember(e.target.checked)} color="primary" />}
                                label="Remember me"
                            />
                            <Button
                                disabled={submitting}
                                variant="contained"
                                color="primary"
                                type="submit"
                                size="large"
                                fullWidth
                            >
                                {submitting ? 'Signing in…' : 'Sign in'}
                            </Button>
                        </Stack>
                    </Box>

                    <Stack direction="row" justifyContent="space-between" sx={{ mt: 2 }}>
                        <MuiLink component="button" type="button" disabled sx={{ opacity: 0.7 }}>
                            Forgot password?
                        </MuiLink>
                        <MuiLink component={RouterLink} to="/register">
                            Create account
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