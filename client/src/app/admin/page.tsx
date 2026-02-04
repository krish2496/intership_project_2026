'use client';

import { useEffect, useState } from 'react';
import api from '@/lib/api';
import { useAuth } from '@/context/AuthContext';
import { useRouter } from 'next/navigation';
import Link from 'next/link';
import { toast } from 'react-toastify';
import { PollCard } from '@/components/PollCard';

interface SystemStats {
    totalUsers: number;
    totalMediaTracked: number;
    totalClubs: number;
    totalDiscussions: number;
    totalPolls: number;
}

interface UserAdminDto {
    id: number;
    username: string;
    email: string;
    role: string;
    createdAt: string;
}

interface PollOption {
    id: number;
    text: string;
    voteCount: number;
}

interface Poll {
    id: number;
    question: string;
    creatorName: string;
    options: PollOption[];
    userVotedOptionId?: number;
    totalVotes: number;
}

export default function AdminPage() {
    const { user, loading: authLoading } = useAuth();
    const router = useRouter();
    const [stats, setStats] = useState<SystemStats | null>(null);
    const [users, setUsers] = useState<UserAdminDto[]>([]);
    const [polls, setPolls] = useState<Poll[]>([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        if (!authLoading) {
            if (!user) {
                router.push('/auth/login');
            } else {
                // Basic role check - ideally verify token claim via API failure
                fetchData();
            }
        }
    }, [user, authLoading]);

    const fetchData = async () => {
        try {
            const statsRes = await api.get('/admin/stats');
            setStats(statsRes.data);
            const usersRes = await api.get('/admin/users');
            setUsers(usersRes.data);
            const pollRes = await api.get('/poll');
            setPolls(pollRes.data);
        } catch (err: any) {
            if (err.response?.status === 403) {
                toast.error("Access Denied: Admins Only");
                router.push('/dashboard');
            } else {
                toast.error("Failed to load admin data");
            }
        } finally {
            setLoading(false);
        }
    };

    const deleteUser = async (id: number) => {
        if (!confirm("Are you sure you want to permanently delete this user? This action cannot be undone.")) return;
        try {
            await api.delete(`/admin/users/${id}`);
            toast.success("User deleted successfully");
            fetchData();
        } catch (err) {
            toast.error("Failed to delete user");
        }
    };

    if (authLoading || loading) return <p className="text-center mt-10">Loading...</p>;

    return (
        <div className="space-y-8">
            <div className="flex justify-between items-center">
                <h1 className="text-3xl font-bold border-l-4 border-red-600 pl-4">Admin Dashboard</h1>
                <Link
                    href="/profile"
                    className="px-4 py-2 bg-gray-700 rounded text-white hover:bg-gray-600 transition"
                >
                    My Profile
                </Link>
            </div>

            {/* Stats Cards */}
            {stats && (
                <div className="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-5 gap-4">
                    <div className="bg-gray-800 p-4 rounded shadow text-center">
                        <h3 className="text-gray-400 text-sm">Total Users</h3>
                        <p className="text-2xl font-bold">{stats.totalUsers}</p>
                    </div>
                    <div className="bg-gray-800 p-4 rounded shadow text-center">
                        <h3 className="text-gray-400 text-sm">Tracked Items</h3>
                        <p className="text-2xl font-bold">{stats.totalMediaTracked}</p>
                    </div>
                    <div className="bg-gray-800 p-4 rounded shadow text-center">
                        <h3 className="text-gray-400 text-sm">Clubs</h3>
                        <p className="text-2xl font-bold">{stats.totalClubs}</p>
                    </div>
                    <div className="bg-gray-800 p-4 rounded shadow text-center">
                        <h3 className="text-gray-400 text-sm">Discussions</h3>
                        <p className="text-2xl font-bold">{stats.totalDiscussions}</p>
                    </div>
                    <div className="bg-gray-800 p-4 rounded shadow text-center">
                        <h3 className="text-gray-400 text-sm">Polls</h3>
                        <p className="text-2xl font-bold">{stats.totalPolls}</p>
                    </div>
                </div>
            )}

            {/* User Management */}
            <div className="bg-gray-800 rounded-lg p-6 shadow">
                <h2 className="text-xl font-bold mb-4">User Management</h2>
                <div className="overflow-x-auto">
                    <table className="w-full text-left">
                        <thead>
                            <tr className="border-b border-gray-700">
                                <th className="p-3">ID</th>
                                <th className="p-3">Username</th>
                                <th className="p-3">Email</th>
                                <th className="p-3">Role</th>
                                <th className="p-3">Joined</th>
                                <th className="p-3">Actions</th>
                            </tr>
                        </thead>
                        <tbody>
                            {users.map(u => (
                                <tr key={u.id} className="border-b border-gray-700 hover:bg-gray-750">
                                    <td className="p-3">{u.id}</td>
                                    <td className="p-3 font-bold">{u.username}</td>
                                    <td className="p-3 text-gray-400">{u.email}</td>
                                    <td className="p-3">
                                        <span className={`px-2 py-1 rounded text-xs ${u.role === 'Admin' ? 'bg-red-900 text-red-200' : 'bg-blue-900 text-blue-200'}`}>
                                            {u.role}
                                        </span>
                                    </td>
                                    <td className="p-3 text-sm text-gray-500">{new Date(u.createdAt).toLocaleDateString()}</td>
                                    <td className="p-3">
                                        {u.role !== 'Admin' && (
                                            <button
                                                onClick={() => deleteUser(u.id)}
                                                className="text-red-500 hover:text-red-400 text-sm font-semibold transition"
                                            >
                                                Delete
                                            </button>
                                        )}
                                    </td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                </div>
            </div>

            {/* Global Polls */}
            <div className="bg-gray-800 rounded-lg p-6 shadow">
                <h2 className="text-xl font-bold mb-4">Global Polls</h2>
                {polls.length > 0 ? (
                    <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
                        {polls.map(poll => (
                            <PollCard key={poll.id} poll={poll} />
                        ))}
                    </div>
                ) : (
                    <p className="text-gray-500 text-center py-4">No active polls available.</p>
                )}
            </div>
        </div>
    );
}
