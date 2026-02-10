'use client';

import { useEffect, useState } from 'react';
import { socialService, LeaderboardEntry } from '@/services/socialService';
import Link from 'next/link';

export default function LeaderboardPage() {
    const [users, setUsers] = useState<LeaderboardEntry[]>([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        fetchLeaderboard();
    }, []);

    const fetchLeaderboard = async () => {
        try {
            const data = await socialService.getLeaderboard();
            console.log('Leaderboard data:', data);
            console.log('First user:', data[0]);
            setUsers(data);
        } catch (error) {
            console.error('Failed to fetch leaderboard:', error);
        } finally {
            setLoading(false);
        }
    };

    if (loading) return <div className="text-center mt-10">Loading leaderboard...</div>;

    return (
        <div className="container mx-auto p-4 max-w-3xl">
            <h1 className="text-3xl font-bold mb-6 text-center text-yellow-400">🏆 Top Watchers</h1>

            <div className="bg-gray-800 rounded-lg shadow overflow-hidden">
                <table className="w-full text-left">
                    <thead className="bg-gray-700 text-gray-300 uppercase text-xs">
                        <tr>
                            <th className="px-6 py-3">Rank</th>
                            <th className="px-6 py-3">User</th>
                            <th className="px-6 py-3 text-right">Episodes Watched</th>
                            <th className="px-6 py-3 text-right">Anime Completed</th>
                        </tr>
                    </thead>
                    <tbody className="divide-y divide-gray-700">
                        {users.map((user, index) => (
                            <tr key={user.id} className="hover:bg-gray-750 transition">
                                <td className="px-6 py-4 whitespace-nowrap">
                                    {index === 0 ? '🥇' : index === 1 ? '🥈' : index === 2 ? '🥉' : `#${index + 1}`}
                                </td>
                                <td className="px-6 py-4 whitespace-nowrap font-medium text-white">
                                    <Link href={`/users/${user.id}`} className="hover:underline text-blue-400">
                                        {user.username}
                                    </Link>
                                </td>
                                <td className="px-6 py-4 whitespace-nowrap text-right text-gray-300">
                                    {user.totalEpisodes}
                                </td>
                                <td className="px-6 py-4 whitespace-nowrap text-right text-gray-300">
                                    {user.animeCompleted}
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            </div>
        </div>
    );
}
