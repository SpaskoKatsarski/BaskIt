import { useNavigate } from 'react-router-dom';
import { useAuthStore } from '../store/authStore';

export function useLogout() {
  const clearAuth = useAuthStore((state) => state.clearAuth);
  const navigate = useNavigate();

  const logout = () => {
    clearAuth();
    navigate('/login');
  };

  return { logout };
}
