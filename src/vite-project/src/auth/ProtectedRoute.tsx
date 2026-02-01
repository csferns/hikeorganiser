import React from 'react';
import { Navigate, useLocation } from 'react-router';
import { useAuth } from './AuthContext.tsx';

const ProtectedRoute: React.FC<React.PropsWithChildren> = ({ children }) => {
    const { isAuthenticated, loading } = useAuth();
    const location = useLocation();

    if (loading) return null; // or a spinner

    if (!isAuthenticated) {
        return <Navigate to="/login" replace state={{ from: location }} />;
    }

    return <>{children}</>;
}

export default ProtectedRoute;
