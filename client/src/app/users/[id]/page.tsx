'use client';

import { useEffect, useState } from 'react';
import { useParams } from 'next/navigation';
import { socialService, Activity } from '@/services/socialService';
import { useAuth } from '@/context/AuthContext';
import { toast } from 'react-toastify';
import Link from 'next/link';

interface PublicProfile {
    id: number;
    username: string;
    createdAt: string;
    followersCount: number;
    followingCount: number;
}

export default function UserProfilePage() {
    const { id } = useParams();
    const userId = parseInt(id as string);
    const { user: currentUser } = useAuth();

    const [profile, setProfile] = useState<PublicProfile | null>(null);
    const [activities, setActivities] = useState<Activity[]>([]);
    const [isFollowing, setIsFollowing] = useState(false);
    const [loading, setLoading] = useState(true);
    const [actionLoading, setActionLoading] = useState(false);

    useEffect(() => {
        if (userId) {
            loadData();
        }
    }, [userId, currentUser]);

    const loadData = async () => {
        try {
            setLoading(true);
            const [profileData, activityData] = await Promise.all([
                socialService.getPublicProfile(userId),
                socialService.getUserActivity(userId)
            ]);

            setProfile(profileData);
            setActivities(activityData);

            // Check if following
            if (currentUser && currentUser.id !== userId) {
                const myFollowing = await socialService.getFollowing(currentUser.id);
                // The API returns {id, username...} objects for following, checking against user ID
                const isFound = myFollowing.some((f: any) => f.id === userId);
                setIsFollowing(isFound);
            }
        } catch (error) {
            console.error('Failed to load user data:', error);
            toast.error('Failed to load user profile');
        } finally {
            setLoading(false);
        }
    };

    const handleFollowToggle = async () => {
        if (!currentUser) return;

        setActionLoading(true);
        try {
            if (isFollowing) {
                await socialService.unfollowUser(userId);
                setIsFollowing(false);
                toast.success(`Unfollowed ${profile?.username}`);
                setProfile(prev => prev ? { ...prev, followersCount: prev.followersCount - 1 } : null);
            } else {
                await socialService.followUser(userId);
                setIsFollowing(true);
                toast.success(`Followed ${profile?.username}`);
                setProfile(prev => prev ? { ...prev, followersCount: prev.followersCount + 1 } : null);
            }
        } catch (error) {
            console.error('Action failed:', error);
            toast.error('Failed to update follow status');
        } finally {
            setActionLoading(false);
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

    if (loading) return <div className="text-center mt-10">Loading profile...</div>;
    if (!profile) return <div className="text-center mt-10">User not found</div>;

    return (
        <div className="container mx-auto p-4 max-w-4xl">
            {/* Header Card */}
            <div className="bg-gray-800 rounded-lg p-6 shadow mb-6 flex flex-col md:flex-row items-center md:items-start space-y-4 md:space-y-0 md:space-x-6">
                <div className="w-24 h-24 bg-blue-600 rounded-full flex items-center justify-center text-4xl font-bold text-white">
                    {profile.username[0].toUpperCase()}
                </div>

                <div className="flex-1 text-center md:text-left">
                    <h1 className="text-3xl font-bold">{profile.username}</h1>
                    <p className="text-gray-400 text-sm">Member since {new Date(profile.createdAt).toLocaleDateString()}</p>

                    <div className="flex justify-center md:justify-start space-x-6 mt-4">
                        <div className="text-center">
                            <span className="block text-xl font-bold text-white">{profile.followersCount}</span>
                            <span className="text-xs text-gray-400">Followers</span>
                        </div>
                        <div className="text-center">
                            <span className="block text-xl font-bold text-white">{profile.followingCount}</span>
                            <span className="text-xs text-gray-400">Following</span>
                        </div>
                    </div>
                </div>

                {currentUser && currentUser.id !== userId && (
                    <button
                        onClick={handleFollowToggle}
                        disabled={actionLoading}
                        className={`px-6 py-2 rounded font-medium transition ${isFollowing
                            ? 'bg-gray-600 hover:bg-gray-500 text-white'
                            : 'bg-blue-600 hover:bg-blue-500 text-white'
                            }`}
                    >
                        {actionLoading ? 'Processing...' : isFollowing ? 'Unfollow' : 'Follow'}
                    </button>
                )}
            </div>

            {/* Content Grid */}
            <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
                {/* Recent Activity */}
                <div className="md:col-span-2">
                    <h2 className="text-xl font-bold mb-4 border-l-4 border-yellow-500 pl-3">Recent Activity</h2>
                    <div className="space-y-4">
                        {activities.length === 0 ? (
                            <p className="text-gray-500 italic">No recent activity.</p>
                        ) : (
                            activities.map((activity) => (
                                <div key={activity.id} className="bg-gray-800 p-4 rounded-lg shadow border border-gray-700">
                                    <p className="text-sm text-gray-300 mb-2">
                                        <span className={getActivityColor(activity.type)}>
                                            {getActivityIcon(activity.type)} {activity.description}
                                        </span>
                                    </p>
                                    <p className="text-xs text-gray-500 mb-3">
                                        {new Date(activity.createdAt).toLocaleString()}
                                    </p>
                                    {activity.media && (
                                        <Link href={`/anime/${activity.media.externalId}?type=${activity.media.type}`} className="block">
                                            <div className="bg-gray-750 p-2 rounded flex items-center space-x-3 hover:bg-gray-700 transition">
                                                {activity.media.coverImageUrl && (
                                                    <img
                                                        src={activity.media.coverImageUrl}
                                                        alt={activity.media.title}
                                                        className="w-8 h-12 object-cover rounded"
                                                    />
                                                )}
                                                <span className="text-sm font-medium text-blue-300">{activity.media.title}</span>
                                            </div>
                                        </Link>
                                    )}
                                </div>
                            ))
                        )}
                    </div>
                </div>

                {/* Sidebar (Maybe Stats later?) */}
                <div className="space-y-6">
                    <div className="bg-gray-800 p-4 rounded-lg shadow">
                        <h3 className="font-bold text-gray-400 mb-2 text-sm uppercase">About</h3>
                        <p className="text-gray-300 text-sm">
                            Joined {new Date(profile.createdAt).toLocaleDateString()}
                        </p>
                    </div>
                </div>
            </div>
        </div>
    );
}
