export interface ChangePasswordRequest {
  email: string,
  contactId: string,
  oldPassword: string,
  newPassword: string,
}