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

    const updateProgress = async (id: number, currentProgress: number) => {
        try {
            await api.put(`/watchlist/${id}`, { progress: currentProgress + 1 });
            fetchWatchlist();
        } catch (err) {
            console.error(err);
        }
    };

    if (authLoading || loading) return <p className="text-center mt-10">Loading...</p>;

    return (
        <div className="space-y-6">
            <h1 className="text-3xl font-bold">My List</h1>

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
                        <div key={item.id} className="bg-gray-800 rounded-lg overflow-hidden shadow-lg flex">
                            <img
                                src={item.media.coverImageUrl || '/placeholder.png'}
                                alt={item.media.title}
                                className="w-32 h-48 object-cover"
                            />
                            <div className="p-4 flex flex-col justify-between flex-1">
                                <div>
                                    <h3 className="text-lg font-bold line-clamp-2">{item.media.title}</h3>
                                    <div className="text-sm text-gray-400 mt-1">
                                        {typeMap[item.media.type]} • {statusMap[item.status]}
                                    </div>
                                </div>

                                <div className="mt-4">
                                    <p className="text-sm mb-2">Progress: {item.progress} / {item.media.totalEpisodes || '?'}</p>
                                    <div className="flex space-x-2">
                                        <button
                                            onClick={() => updateProgress(item.id, item.progress)}
                                            className="bg-green-600 hover:bg-green-700 text-white px-3 py-1 rounded text-sm transition"
                                        >
                                            +1 Ep
                                        </button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    ))}
                </div>
            )}
        </div>
    );
}
