import React, { createContext, useCallback, useContext, useEffect, useMemo, useState } from 'react';
import { apiService } from '../defaults.ts';

export interface AuthUserInfo {
    email?: string;
    userName?: string;
    id?: string;
    [key: string]: any;
}

interface AuthContextValue {
    user: AuthUserInfo | null;
    loading: boolean;
    isAuthenticated: boolean;
    login: (email: string, password: string) => Promise<void>;
    logout: () => Promise<void>;
    refreshUser: () => Promise<void>;
}

const AuthContext = createContext<AuthContextValue | undefined>(undefined);

export const AuthProvider: React.FC<React.PropsWithChildren> = ({ children }) => {
    const [user, setUser] = useState<AuthUserInfo | null>(null);
    const [loading, setLoading] = useState<boolean>(true);

    const refreshUser = useCallback(async () => {
        try {
            const info = await apiService.getUserInfo();
            setUser(info ?? null);
        } catch (_) {
            setUser(null);
        } finally {
            setLoading(false);
        }
    }, []);

    useEffect(() => {
        // Try to load current user on mount (cookie-based auth)
        refreshUser();
    }, [refreshUser]);

    const login = useCallback(async (email: string, password: string) => {
        await apiService.login(email, password);
        await refreshUser();
    }, [refreshUser]);

    const logout = useCallback(async () => {
        await apiService.logout();
        setUser(null);
    }, []);

    const value = useMemo<AuthContextValue>(() => ({
        user,
        loading,
        isAuthenticated: !!user,
        login,
        logout,
        refreshUser
    }), [user, loading, login, logout, refreshUser]);

    return (
        <AuthContext.Provider value={value}>
            {children}
        </AuthContext.Provider>
    );
};

export const useAuth = () => {
    const ctx = useContext(AuthContext);
    if (!ctx) throw new Error('useAuth must be used within AuthProvider');
    return ctx;
}
