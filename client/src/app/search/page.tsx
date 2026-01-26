'use client';

import { useState } from 'react';
import api from '@/lib/api';
import { toast } from 'react-toastify';
import { useAuth } from '@/context/AuthContext';

interface MediaResult {
    title: string;
    coverImageUrl: string;
    externalId: number;
    type: number; // 0 = Anime, 1 = TV
    totalEpisodes?: number;
    description: string;
}

export default function SearchPage() {
    const [query, setQuery] = useState('');
    const [type, setType] = useState(0); // 0 = Anime
    const [results, setResults] = useState<MediaResult[]>([]);
    const [loading, setLoading] = useState(false);
    const { user } = useAuth();

    const handleSearch = async (e: React.FormEvent) => {
        e.preventDefault();
        if (!query.trim()) return;

        setLoading(true);
        try {
            const response = await api.get(`/media/search?query=${query}&type=${type}`);
            setResults(response.data);
        } catch (err) {
            toast.error('Search failed');
        } finally {
            setLoading(false);
        }
    };

    const addToWatchlist = async (media: MediaResult) => {
        if (!user) {
            toast.info("Please login to add to list");
            return;
        }
        try {
            await api.post('/watchlist', {
                mediaExternalId: media.externalId,
                mediaType: media.type,
                title: media.title,
                coverImageUrl: media.coverImageUrl,
                totalEpisodes: media.totalEpisodes,
                description: media.description,
                status: 4 // Plan to Watch
            });
            toast.success('Added to list');
        } catch (err: any) {
            toast.error(err.response?.data || 'Failed to add');
        }
    };

    return (
        <div className="space-y-6">
            <h1 className="text-3xl font-bold">Search</h1>

            <form onSubmit={handleSearch} className="flex gap-4">
                <input
                    type="text"
                    value={query}
                    onChange={(e) => setQuery(e.target.value)}
                    placeholder="Search for anime or TV shows..."
                    className="flex-1 p-3 bg-gray-800 rounded text-white focus:outline-none focus:ring-2 focus:ring-blue-500"
                />
                <select
                    value={type}
                    onChange={(e) => setType(Number(e.target.value))}
                    className="p-3 bg-gray-800 rounded text-white focus:outline-none focus:ring-2 focus:ring-blue-500"
                >
                    <option value={0}>Anime</option>
                    <option value={1}>TV Series</option>
                </select>
                <button
                    type="submit"
                    disabled={loading}
                    className="px-6 bg-blue-600 hover:bg-blue-700 rounded text-white font-semibold disabled:opacity-50"
                >
                    {loading ? 'Searching...' : 'Search'}
                </button>
            </form>

            <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-6">
                {results.map((item) => (
                    <div key={`${item.type}-${item.externalId}`} className="bg-gray-800 rounded-lg overflow-hidden shadow-lg group relative">
                        <div className="relative h-64 overflow-hidden">
                            <img
                                src={item.coverImageUrl || '/placeholder.png'}
                                alt={item.title}
                                className="w-full h-full object-cover transition transform group-hover:scale-110"
                            />
                            <div className="absolute inset-0 bg-black bg-opacity-0 group-hover:bg-opacity-60 transition flex items-center justify-center opacity-0 group-hover:opacity-100">
                                <button
                                    onClick={() => addToWatchlist(item)}
                                    className="bg-blue-600 text-white px-4 py-2 rounded-full font-bold shadow-lg transform translate-y-4 group-hover:translate-y-0 transition"
                                >
                                    + Add to List
                                </button>
                            </div>
                        </div>
                        <div className="p-4">
                            <h3 className="font-bold truncate" title={item.title}>{item.title}</h3>
                            <p className="text-sm text-gray-400 mt-1 line-clamp-2">{item.description}</p>
                        </div>
                    </div>
                ))}
            </div>
        </div>
    );
}
