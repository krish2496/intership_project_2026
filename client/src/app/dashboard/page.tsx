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

                                    {/* Rating */}
                                    <div>
                                        <label className="text-xs text-gray-400 block mb-1">
                                            Rating {item.rating ? `(${item.rating}/10)` : ''}
                                        </label>
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
