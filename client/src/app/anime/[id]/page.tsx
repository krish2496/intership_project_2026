'use client';

import { useEffect, useState } from 'react';
import { useParams, useSearchParams, useRouter } from 'next/navigation';
import api from '@/lib/api';
import { toast } from 'react-toastify';
import { useAuth } from '@/context/AuthContext';

interface Media {
    title: string;
    description: string;
    coverImageUrl: string;
    totalEpisodes: number;
    externalId: number;
    type: number;
}

export default function AnimeDetailsPage() {
    const { id } = useParams();
    const searchParams = useSearchParams();
    const type = searchParams.get('type') || '0';
    const [media, setMedia] = useState<Media | null>(null);
    const [loading, setLoading] = useState(true);
    const router = useRouter();
    const { user } = useAuth();

    useEffect(() => {
        if (!id) return;

        const fetchDetails = async () => {
            try {
                const response = await api.get(`/media/${id}?type=${type}`);
                setMedia(response.data);
            } catch (error) {
                console.error("Error fetching details:", error);
                toast.error("Failed to load anime details");
            } finally {
                setLoading(false);
            }
        };

        fetchDetails();
    }, [id, type]);

    const addToWatchlist = async () => {
        if (!user) {
            toast.info("Please login to add to list");
            return;
        }
        if (!media) return;

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
            toast.error(err.response?.data || 'Failed to add to list');
        }
    };

    if (loading) return <div className="text-white text-center mt-10">Loading...</div>;
    if (!media) return <div className="text-white text-center mt-10">Anime not found</div>;

    return (
        <div className="container mx-auto px-4 py-8 text-white">
            <button onClick={() => router.back()} className="mb-6 text-blue-400 hover:text-blue-300 transition flex items-center gap-2">
                <span>&larr;</span> Back to Search
            </button>
            <div className="flex flex-col md:flex-row gap-8 bg-gray-900/50 p-8 rounded-xl shadow-2xl backdrop-blur-sm">
                <div className="w-full md:w-1/3 lg:w-1/4 flex-shrink-0">
                    <img 
                        src={media.coverImageUrl || '/placeholder.png'} 
                        alt={media.title} 
                        className="w-full rounded-lg shadow-lg object-cover aspect-[2/3]"
                    />
                    <button 
                        onClick={addToWatchlist}
                        className="w-full mt-6 bg-blue-600 hover:bg-blue-700 text-white font-bold py-3 px-6 rounded-lg transition transform hover:scale-105 active:scale-95 shadow-lg"
                    >
                        Add to Watchlist
                    </button>
                </div>
                <div className="w-full md:w-2/3 lg:w-3/4">
                    <h1 className="text-4xl md:text-5xl font-bold mb-4 bg-clip-text text-transparent bg-gradient-to-r from-blue-400 to-purple-600">{media.title}</h1>
                    <div className="flex flex-wrap gap-3 text-sm text-gray-400 mb-8">
                        <span className="px-3 py-1 bg-gray-800 rounded-full border border-gray-700">
                             {media.type === 0 ? 'Anime' : 'TV Series'}
                        </span>
                        {media.totalEpisodes && (
                            <span className="px-3 py-1 bg-gray-800 rounded-full border border-gray-700">
                                {media.totalEpisodes} Episodes
                            </span>
                        )}
                    </div>
                    
                    <div className="prose prose-invert max-w-none">
                        <h3 className="text-xl font-semibold mb-3 text-gray-200">Synopsis</h3>
                        <p className="text-gray-300 leading-relaxed text-lg whitespace-pre-wrap">
                            {media.description}
                        </p>
                    </div>
                </div>
            </div>
        </div>
    );
}
