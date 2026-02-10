'use client';

import { useEffect, useState } from 'react';
import { socialService, Activity } from '@/services/socialService';
import { useAuth } from '@/context/AuthContext';
import Link from 'next/link';

export default function FeedPage() {
    const { user, loading: authLoading } = useAuth();
    const [activities, setActivities] = useState<Activity[]>([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        if (!authLoading && user) {
            fetchFeed();
        }
    }, [user, authLoading]);

    const fetchFeed = async () => {
        try {
            const data = await socialService.getFeed();
            setActivities(data);
        } catch (error) {
            console.error('Failed to fetch feed:', error);
        } finally {
            setLoading(false);
        }
    };

    const getActivityIcon = (type: number) => {
        switch (type) {
            case 0: return '📺'; // Watched
            case 1: return '🏆'; // Completed
            case 2: return '⭐'; // Rated
            case 3: return '👥'; // Joined Club
            default: return '📝';
        }
    };

    const getActivityColor = (type: number) => {
        switch (type) {
            case 0: return 'text-blue-400';
            case 1: return 'text-yellow-400';
            case 2: return 'text-purple-400';
            case 3: return 'text-green-400';
            default: return 'text-gray-400';
        }
    };

    if (authLoading || loading) return <div className="text-center mt-10">Loading feed...</div>;

    return (
        <div className="container mx-auto p-4 max-w-2xl">
            <h1 className="text-2xl font-bold mb-6">Activity Feed</h1>

            <div className="space-y-4">
                {activities.length === 0 ? (
                    <p className="text-gray-400 text-center">No activity yet. Follow some users to see updates!</p>
                ) : (
                    activities.map((activity) => (
                        <div key={activity.id} className="bg-gray-800 p-4 rounded-lg shadow flex items-start space-x-4">
                            <div className="flex-shrink-0">
                                <div className="w-10 h-10 bg-gray-700 rounded-full flex items-center justify-center text-lg">
                                    {activity.user.username[0].toUpperCase()}
                                </div>
                            </div>
                            <div className="flex-1">
                                <p className="text-sm text-gray-300">
                                    <span className="font-bold text-white">{activity.user.username}</span>
                                    {' '}
                                    <span className={getActivityColor(activity.type)}>
                                        {getActivityIcon(activity.type)} {activity.description}
                                    </span>
                                </p>
                                <p className="text-xs text-gray-500 mt-1">
                                    {new Date(activity.createdAt).toLocaleString()}
                                </p>
                                {activity.media && (
                                    <Link href={`/anime/${activity.media.externalId}?type=${activity.media.type}`} className="block mt-2">
                                        <div className="bg-gray-700 p-2 rounded flex items-center space-x-3 hover:bg-gray-600 transition">
                                            {activity.media.coverImageUrl && (
                                                <img
                                                    src={activity.media.coverImageUrl}
                                                    alt={activity.media.title}
                                                    className="w-10 h-14 object-cover rounded"
                                                />
                                            )}
                                            <span className="text-sm font-medium">{activity.media.title}</span>
                                        </div>
                                    </Link>
                                )}
                            </div>
                        </div>
                    ))
                )}
            </div>
        </div>
    );
}
