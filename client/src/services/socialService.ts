import api from '@/lib/api';

export interface User {
    id: number;
    username: string;
    email: string;
    profileImage?: string;
}

export interface Activity {
    id: number;
    userId: number;
    user: User;
    type: number; // 0: WatchedEpisode, 1: CompletedShow, 2: RatedShow, 3: JoinedClub
    mediaId?: number;
    media?: {
        id: number;
        title: string;
        coverImageUrl: string;
        externalId: number;
        type: number;
    };
    description: string;
    createdAt: string;
}

export interface LeaderboardEntry {
    id: number;
    username: string;
    totalEpisodes: number;
    animeCompleted: number;
}

export const socialService = {
    followUser: async (id: number) => {
        const response = await api.post(`/social/follow/${id}`);
        return response.data;
    },

    unfollowUser: async (id: number) => {
        const response = await api.post(`/social/unfollow/${id}`);
        return response.data;
    },

    getFollowers: async (id: number) => {
        const response = await api.get(`/social/followers/${id}`);
        return response.data;
    },

    getFollowing: async (id: number) => {
        const response = await api.get(`/social/following/${id}`);
        return response.data;
    },

    getFeed: async () => {
        const response = await api.get('/feed');
        return response.data;
    },

    getUserActivity: async (id: number) => {
        const response = await api.get(`/feed/user/${id}`);
        return response.data;
    },

    getLeaderboard: async () => {
        const response = await api.get('/leaderboard/top-watchers');
        return response.data;
    },

    getPublicProfile: async (id: number) => {
        const response = await api.get(`/social/profile/${id}`);
        return response.data;
    }
};
