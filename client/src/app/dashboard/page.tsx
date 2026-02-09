'use client';

import { useEffect, useState } from 'react';
import api from '@/lib/api';
import { useAuth } from '@/context/AuthContext';
import { useRouter } from 'next/navigation';
import Link from 'next/link';
import { toast } from 'react-toastify';

interface WatchlistDto {
    id: number;
    mediaId: number;
    media: {
        title: string;
        coverImageUrl: string;
        type: number;
        totalEpisodes?: number;
    };
    status: number;
    progress: number;
    rating?: number;
}

const statusMap = ['Watching', 'Completed', 'On Hold', 'Dropped', 'Plan to Watch'];
const typeMap = ['Anime', 'TV Series'];

export default function DashboardPage() {
    const { user, loading: authLoading } = useAuth();
    const router = useRouter();
    const [watchlist, setWatchlist] = useState<WatchlistDto[]>([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        if (!authLoading && !user) {
            router.push('/auth/login');
        } else if (user) {
            fetchWatchlist();
        }
    }, [user, authLoading, router]);

    const fetchWatchlist = async () => {
        try {
            const response = await api.get('/watchlist');
            setWatchlist(response.data);
        } catch (err) {
            console.error(err);
        } finally {
            setLoading(false);
        }
    };

    const updateProgress = async (id: number, currentProgress: number, totalEpisodes?: number) => {
        // Check if we've reached the limit
        if (totalEpisodes && currentProgress >= totalEpisodes) {
            toast.info('You\'ve already watched all episodes!');
            return;
        }

        try {
            await api.put(`/watchlist/${id}`, { progress: currentProgress + 1 });
            toast.success('Progress updated!');
            fetchWatchlist();
        } catch (err) {
            console.error(err);
            toast.error('Failed to update');
        }
    };

    const updateStatus = async (id: number, newStatus: number) => {
        try {
            await api.put(`/watchlist/${id}`, { status: newStatus });
            toast.success('Status updated!');
            fetchWatchlist();
        } catch (err) {
            console.error(err);
            toast.error('Failed to update');
        }
    };

    const updateRating = async (id: number, rating: number) => {
        try {
            await api.put(`/watchlist/${id}`, { rating });
            toast.success(`Rated ${rating}/10!`);
            fetchWatchlist();
        } catch (err) {
            console.error(err);
            toast.error('Failed to rate');
        }
    };

    const deleteItem = async (id: number) => {
        try {
            await api.delete(`/watchlist/${id}`);
            toast.success('Removed from watchlist');
            fetchWatchlist();
        } catch (err) {
            console.error(err);
            toast.error('Failed to remove item');
        }
    };

    if (authLoading || loading) return <p className="text-center mt-10">Loading...</p>;

    return (
        <div className="space-y-8">
            {/* Watchlist Section */}
            <div>
                <h1 className="text-3xl font-bold mb-4">My List</h1>

                {watchlist.length === 0 ? (
                    <div className="text-center py-20 bg-gray-800 rounded-lg">
                        <p className="text-xl text-gray-400 mb-4">You haven't added anything yet.</p>
                        <Link href="/search" className="px-6 py-3 bg-blue-600 rounded text-white hover:bg-blue-700">
                            Discover Anime & TV
                        </Link>
                    </div>
                ) : (
                    <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
                        {watchlist.map((item) => (
                            <div key={item.id} className="bg-gray-800 rounded-lg overflow-hidden shadow-lg flex flex-col">
                                <div className="flex">
                                    <img
                                        src={item.media.coverImageUrl || '/placeholder.png'}
                                        alt={item.media.title}
                                        className="w-32 h-48 object-cover"
                                    />
                                    <div className="p-4 flex flex-col justify-between flex-1">
                                        <div>
                                            <h3 className="text-lg font-bold line-clamp-2">{item.media.title}</h3>
                                            <div className="text-sm text-gray-400 mt-1">
                                                {typeMap[item.media.type]}
                                            </div>
                                        </div>

                                        <div className="mt-2">
                                            <p className="text-sm mb-2">Progress: {item.progress} / {item.media.totalEpisodes || '?'}</p>
                                            <button
                                                onClick={() => updateProgress(item.id, item.progress, item.media.totalEpisodes)}
                                                className="bg-green-600 hover:bg-green-700 text-white px-3 py-1 rounded text-sm transition w-full"
                                            >
                                                +1 Ep
                                            </button>
                                        </div>
                                    </div>
                                </div>

                                {/* Status and Rating Controls */}
                                <div className="p-4 border-t border-gray-700 space-y-3">
                                    {/* Status Dropdown */}
                                    <div>
                                        <label className="text-xs text-gray-400 block mb-1">Status</label>
                                        <select
                                            value={item.status}
                                            onChange={(e) => updateStatus(item.id, Number(e.target.value))}
                                            className="w-full bg-gray-700 text-white px-3 py-2 rounded text-sm focus:outline-none focus:ring-2 focus:ring-purple-500"
                                        >
                                            {statusMap.map((status, index) => (
                                                <option key={index} value={index}>{status}</option>
                                            ))}
                                        </select>
                                    </div>

                                    {/* Rating and Delete */}
                                    <div>
                                        <div className="flex justify-between items-center mb-1">
                                            <label className="text-xs text-gray-400">
                                                Rating {item.rating ? `(${item.rating}/10)` : ''}
                                            </label>
                                            <button
                                                onClick={() => {
                                                    if (confirm('Are you sure you want to remove this from your list?')) {
                                                        deleteItem(item.id);
                                                    }
                                                }}
                                                className="text-red-400 hover:text-red-300 text-xs flex items-center gap-1 transition-colors"
                                                title="Remove from list"
                                            >
                                                <svg xmlns="http://www.w3.org/2000/svg" className="h-4 w-4" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                                                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16" />
                                                </svg>
                                                Delete
                                            </button>
                                        </div>
                                        <div className="flex gap-1 flex-wrap">
                                            {[1, 2, 3, 4, 5, 6, 7, 8, 9, 10].map((rating) => (
                                                <button
                                                    key={rating}
                                                    onClick={() => updateRating(item.id, rating)}
                                                    className={`px-2 py-1 rounded text-xs transition ${item.rating === rating
                                                        ? 'bg-yellow-500 text-black font-bold'
                                                        : 'bg-gray-700 hover:bg-gray-600 text-white'
                                                        }`}
                                                >
                                                    {rating}
                                                </button>
                                            ))}
                                        </div>
                                    </div>
                                </div>
                            </div>
                        ))}
                    </div>
                )}
            </div>
        </div>
    );
}
