'use client';

import { useEffect, useState } from 'react';
import api from '@/lib/api';
import { useAuth } from '@/context/AuthContext';
import { useRouter } from 'next/navigation';
import { toast } from 'react-toastify';

interface ProfileData {
    username: string;
    email: string;
    role: string;
    themePreference: string;
    createdAt: string;
}

export default function ProfilePage() {
    const { user, loading: authLoading } = useAuth();
    const router = useRouter();
    const [profile, setProfile] = useState<ProfileData | null>(null);
    const [loading, setLoading] = useState(true);

    // Password change form
    const [currentPassword, setCurrentPassword] = useState('');
    const [newPassword, setNewPassword] = useState('');
    const [confirmPassword, setConfirmPassword] = useState('');
    const [changingPassword, setChangingPassword] = useState(false);

    // Profile update form
    const [editMode, setEditMode] = useState(false);
    const [editUsername, setEditUsername] = useState('');
    const [editEmail, setEditEmail] = useState('');
    const [editTheme, setEditTheme] = useState('');
    const [updatingProfile, setUpdatingProfile] = useState(false);

    useEffect(() => {
        if (!authLoading && !user) {
            router.push('/auth/login');
        } else if (user) {
            fetchProfile();
        }
    }, [user, authLoading, router]);

    const fetchProfile = async () => {
        try {
            const response = await api.get('/profile');
            setProfile(response.data);
            setEditUsername(response.data.username);
            setEditEmail(response.data.email);
            setEditTheme(response.data.themePreference);
        } catch (err: any) {
            toast.error('Failed to load profile');
            console.error(err);
        } finally {
            setLoading(false);
        }
    };

    const handlePasswordChange = async (e: React.FormEvent) => {
        e.preventDefault();

        if (newPassword !== confirmPassword) {
            toast.error('New passwords do not match');
            return;
        }

        if (newPassword.length < 6) {
            toast.error('Password must be at least 6 characters');
            return;
        }

        setChangingPassword(true);
        try {
            await api.put('/profile/password', {
                currentPassword,
                newPassword
            });
            toast.success('Password changed successfully');
            setCurrentPassword('');
            setNewPassword('');
            setConfirmPassword('');
        } catch (err: any) {
            toast.error(err.response?.data || 'Failed to change password');
        } finally {
            setChangingPassword(false);
        }
    };

    const handleProfileUpdate = async (e: React.FormEvent) => {
        e.preventDefault();
        setUpdatingProfile(true);
        try {
            const response = await api.put('/profile', {
                username: editUsername !== profile?.username ? editUsername : undefined,
                email: editEmail !== profile?.email ? editEmail : undefined,
                themePreference: editTheme !== profile?.themePreference ? editTheme : undefined
            });
            setProfile(response.data);
            toast.success('Profile updated successfully');
            setEditMode(false);
        } catch (err: any) {
            toast.error(err.response?.data || 'Failed to update profile');
        } finally {
            setUpdatingProfile(false);
        }
    };

    if (authLoading || loading) return <p className="text-center mt-10">Loading...</p>;

    return (
        <div className="space-y-8">
            <h1 className="text-3xl font-bold border-l-4 border-blue-600 pl-4">My Profile</h1>

            {/* Profile Information */}
            <div className="bg-gray-800 rounded-lg p-6 shadow">
                <div className="flex justify-between items-center mb-4">
                    <h2 className="text-xl font-bold">Profile Information</h2>
                    {!editMode && (
                        <button
                            onClick={() => setEditMode(true)}
                            className="px-4 py-2 bg-blue-600 rounded text-white hover:bg-blue-700 transition"
                        >
                            Edit Profile
                        </button>
                    )}
                </div>

                {!editMode ? (
                    <div className="space-y-4">
                        <div>
                            <label className="text-gray-400 text-sm">Username</label>
                            <p className="text-lg font-semibold">{profile?.username}</p>
                        </div>
                        <div>
                            <label className="text-gray-400 text-sm">Email</label>
                            <p className="text-lg">{profile?.email}</p>
                        </div>
                        <div>
                            <label className="text-gray-400 text-sm">Role</label>
                            <p className="text-lg">
                                <span className={`px-2 py-1 rounded text-xs ${profile?.role === 'Admin' ? 'bg-red-900 text-red-200' : 'bg-blue-900 text-blue-200'}`}>
                                    {profile?.role}
                                </span>
                            </p>
                        </div>
                        <div>
                            <label className="text-gray-400 text-sm">Theme Preference</label>
                            <p className="text-lg">{profile?.themePreference}</p>
                        </div>
                        <div>
                            <label className="text-gray-400 text-sm">Member Since</label>
                            <p className="text-lg">{profile?.createdAt ? new Date(profile.createdAt).toLocaleDateString() : 'N/A'}</p>
                        </div>
                    </div>
                ) : (
                    <form onSubmit={handleProfileUpdate} className="space-y-4">
                        <div>
                            <label className="block text-gray-400 text-sm mb-1">Username</label>
                            <input
                                type="text"
                                value={editUsername}
                                onChange={(e) => setEditUsername(e.target.value)}
                                className="w-full px-4 py-2 bg-gray-700 rounded border border-gray-600 focus:border-blue-500 focus:outline-none"
                                required
                            />
                        </div>
                        <div>
                            <label className="block text-gray-400 text-sm mb-1">Email</label>
                            <input
                                type="email"
                                value={editEmail}
                                onChange={(e) => setEditEmail(e.target.value)}
                                className="w-full px-4 py-2 bg-gray-700 rounded border border-gray-600 focus:border-blue-500 focus:outline-none"
                                required
                            />
                        </div>
                        <div>
                            <label className="block text-gray-400 text-sm mb-1">Theme Preference</label>
                            <select
                                value={editTheme}
                                onChange={(e) => setEditTheme(e.target.value)}
                                className="w-full px-4 py-2 bg-gray-700 rounded border border-gray-600 focus:border-blue-500 focus:outline-none"
                            >
                                <option value="Light">Light</option>
                                <option value="Dark">Dark</option>
                            </select>
                        </div>
                        <div className="flex space-x-3">
                            <button
                                type="submit"
                                disabled={updatingProfile}
                                className="px-4 py-2 bg-blue-600 rounded text-white hover:bg-blue-700 transition disabled:opacity-50"
                            >
                                {updatingProfile ? 'Saving...' : 'Save Changes'}
                            </button>
                            <button
                                type="button"
                                onClick={() => {
                                    setEditMode(false);
                                    setEditUsername(profile?.username || '');
                                    setEditEmail(profile?.email || '');
                                    setEditTheme(profile?.themePreference || 'Light');
                                }}
                                className="px-4 py-2 bg-gray-600 rounded text-white hover:bg-gray-700 transition"
                            >
                                Cancel
                            </button>
                        </div>
                    </form>
                )}
            </div>

            {/* Change Password */}
            <div className="bg-gray-800 rounded-lg p-6 shadow">
                <h2 className="text-xl font-bold mb-4">Change Password</h2>
                <form onSubmit={handlePasswordChange} className="space-y-4 max-w-md">
                    <div>
                        <label className="block text-gray-400 text-sm mb-1">Current Password</label>
                        <input
                            type="password"
                            value={currentPassword}
                            onChange={(e) => setCurrentPassword(e.target.value)}
                            className="w-full px-4 py-2 bg-gray-700 rounded border border-gray-600 focus:border-blue-500 focus:outline-none"
                            required
                        />
                    </div>
                    <div>
                        <label className="block text-gray-400 text-sm mb-1">New Password</label>
                        <input
                            type="password"
                            value={newPassword}
                            onChange={(e) => setNewPassword(e.target.value)}
                            className="w-full px-4 py-2 bg-gray-700 rounded border border-gray-600 focus:border-blue-500 focus:outline-none"
                            required
                            minLength={6}
                        />
                    </div>
                    <div>
                        <label className="block text-gray-400 text-sm mb-1">Confirm New Password</label>
                        <input
                            type="password"
                            value={confirmPassword}
                            onChange={(e) => setConfirmPassword(e.target.value)}
                            className="w-full px-4 py-2 bg-gray-700 rounded border border-gray-600 focus:border-blue-500 focus:outline-none"
                            required
                            minLength={6}
                        />
                    </div>
                    <button
                        type="submit"
                        disabled={changingPassword}
                        className="px-6 py-2 bg-blue-600 rounded text-white hover:bg-blue-700 transition disabled:opacity-50"
                    >
                        {changingPassword ? 'Changing...' : 'Change Password'}
                    </button>
                </form>
            </div>
        </div>
    );
}
