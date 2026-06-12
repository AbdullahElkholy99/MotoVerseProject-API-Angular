import { jwtDecode } from 'jwt-decode';


export function getUserRole(): string[] {
  const token = localStorage.getItem('token');

  if (!token) return [];

  const decoded: any = jwtDecode(token);

  const roles =
    decoded.role ||
    decoded.Role ||
    decoded['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];

  return Array.isArray(roles) ? roles : [roles];
}

export function getUserInfo(): UserInfo[] {
  const token = localStorage.getItem('token');

  if (!token) return [];

  const decoded: any = jwtDecode(token);

  const imagePath = `https://localhost:7081/` +
    (decoded.imagePath ||
      decoded.ImagePath ||
      decoded['http://schemas.microsoft.com/ws/2008/06/identity/claims/ImagePath']);

  const roles =
    decoded.role ||
    decoded.Role ||
    decoded['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];

  const UserInfo: UserInfo = {
    displayName: decoded.name || decoded.Name || decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'] || '',
    email: decoded.email || decoded.Email || decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'] || '',
    imagePath: imagePath,
    roles: Array.isArray(roles) ? roles : [roles]
  };
  return [UserInfo];
}

interface UserInfo {
  displayName: string
  email: string
  imagePath: string
  roles: string[]
}
