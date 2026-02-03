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

    // Debug: Log initial poll state
    console.log('PollCard loaded with poll:', {
        id: poll.id,
        question: poll.question,
        totalVotes: poll.totalVotes,
        userVotedOptionId: poll.userVotedOptionId,
        options: poll.options.map(o => ({ id: o.id, text: o.text, voteCount: o.voteCount }))
    });

    const handleVote = async (optionId: number) => {
        console.log('handleVote called for option:', optionId, 'userVotedOptionId:', poll.userVotedOptionId);

        // Check if clicking the same option they already voted for
        if (poll.userVotedOptionId === optionId) {
            toast.info('You already voted for this option');
            return;
        }

        setLoading(true);
        try {
            const isChangingVote = poll.userVotedOptionId != null;
            console.log('Voting for option:', optionId, 'on poll:', poll.id, 'isChangingVote:', isChangingVote);

            const response = await api.post(`/poll/${poll.id}/vote`, { optionId });
            console.log('Vote response:', response.data);
            setPoll(response.data);

            if (isChangingVote) {
                toast.success('Vote changed successfully!');
            } else {
                toast.success('Vote cast successfully!');
            }
        } catch (err: any) {
            console.error('Vote error:', err);
            toast.error(err.response?.data || 'Failed to vote');
        } finally {
            setLoading(false);
        }
    };

    const calculatePercentage = (votes: number) => {
        if (poll.totalVotes === 0) return 0;
        return Math.round((votes / poll.totalVotes) * 100);
    };

    const hasVoted = poll.userVotedOptionId != null; // true if user has voted (not null and not undefined)

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
                            {hasVoted && (
                                <div
                                    className="absolute inset-0 bg-blue-900 opacity-20 rounded"
                                    style={{ width: `${percentage}%` }}
                                ></div>
                            )}

                            <button
                                onClick={() => handleVote(option.id)}
                                disabled={loading}
                                className={`w-full text-left p-3 rounded border transition flex justify-between items-center relative z-10 ${isSelected
                                        ? 'border-blue-500 bg-blue-900 bg-opacity-30'
                                        : 'border-gray-600 hover:bg-gray-700 cursor-pointer'
                                    }`}
                            >
                                <span>{option.text}</span>
                                {hasVoted && (
                                    <span className="font-bold">{percentage}% ({option.voteCount} votes)</span>
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
