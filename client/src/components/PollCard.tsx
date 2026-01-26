'use client';

import { useState } from 'react';
import api from '@/lib/api';
import { toast } from 'react-toastify';

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

export function PollCard({ poll: initialPoll }: { poll: Poll }) {
    const [poll, setPoll] = useState(initialPoll);
    const [loading, setLoading] = useState(false);

    const handleVote = async (optionId: number) => {
        if (poll.userVotedOptionId) return;
        setLoading(true);
        try {
            const response = await api.post(`/poll/${poll.id}/vote`, { optionId });
            setPoll(response.data);
            toast.success('Vote cast!');
        } catch (err: any) {
            toast.error(err.response?.data || 'Failed to vote');
        } finally {
            setLoading(false);
        }
    };

    const calculatePercentage = (votes: number) => {
        if (poll.totalVotes === 0) return 0;
        return Math.round((votes / poll.totalVotes) * 100);
    };

    return (
        <div className="bg-gray-800 p-6 rounded-lg shadow-lg border border-gray-700">
            <h3 className="font-bold text-lg mb-2">{poll.question}</h3>
            <p className="text-xs text-gray-500 mb-4">Created by {poll.creatorName}</p>

            <div className="space-y-3">
                {poll.options.map((option) => {
                    const isSelected = poll.userVotedOptionId === option.id;
                    const percentage = calculatePercentage(option.voteCount);

                    return (
                        <div key={option.id} className="relative">
                            {/* Background Bar */}
                            {poll.userVotedOptionId !== undefined && (
                                <div
                                    className="absolute inset-0 bg-blue-900 opacity-20 rx-1 rounded"
                                    style={{ width: `${percentage}%` }}
                                ></div>
                            )}

                            <button
                                onClick={() => handleVote(option.id)}
                                disabled={poll.userVotedOptionId !== undefined || loading}
                                className={`w-full text-left p-3 rounded border transition flex justify-between items-center relative z-10 ${isSelected
                                        ? 'border-blue-500 bg-blue-900 bg-opacity-30'
                                        : 'border-gray-600 hover:bg-gray-700'
                                    }`}
                            >
                                <span>{option.text}</span>
                                {poll.userVotedOptionId !== undefined && (
                                    <span className="font-bold">{percentage}%</span>
                                )}
                            </button>
                        </div>
                    );
                })}
            </div>
            <div className="mt-4 text-xs text-right text-gray-400">
                Total Votes: {poll.totalVotes}
            </div>
        </div>
    );
}
