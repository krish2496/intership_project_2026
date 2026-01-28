'use client';

import { useEffect, useState, use } from 'react';
import api from '@/lib/api';
import { useAuth } from '@/context/AuthContext';
import { useForm } from 'react-hook-form';
import { toast } from 'react-toastify';

interface Comment {
    id: number;
    userName: string;
    content: string;
    isSpoiler: boolean;
    createdAt: string;
    replies: Comment[];
}

interface Discussion {
    id: number;
    title: string;
    content: string;
    userName: string;
    createdAt: string;
}

export default function DiscussionClient({ discussionId }: { discussionId: string }) {
    const [discussion, setDiscussion] = useState<Discussion | null>(null);
    const [comments, setComments] = useState<Comment[]>([]);
    const { user } = useAuth();

    // Create a separate component for the reply form to handle state cleanly
    const ReplyForm = ({ parentId, onCancel }: { parentId?: number, onCancel?: () => void }) => {
        const { register, handleSubmit, reset } = useForm();

        const onSubmit = async (data: any) => {
            try {
                await api.post(`/comment/discussion/${discussionId}`, { ...data, parentCommentId: parentId });
                toast.success('Reply posted');
                reset();
                if (onCancel) onCancel();
                fetchComments();
            } catch (err) {
                toast.error('Failed to post');
            }
        };

        return (
            <form onSubmit={handleSubmit(onSubmit)} className="mt-2 space-y-2">
                <textarea {...register('content', { required: true })} className="w-full p-2 bg-gray-700 rounded text-sm text-white" placeholder="Write a reply..." />
                <div className="flex justify-end gap-2">
                    {onCancel && <button type="button" onClick={onCancel} className="text-xs text-gray-400">Cancel</button>}
                    <button type="submit" className="bg-blue-600 px-3 py-1 rounded text-xs text-white">Reply</button>
                </div>
            </form>
        );
    };

    useEffect(() => {
        fetchData();
    }, [discussionId]);

    const fetchData = async () => {
        try {
            const discRes = await api.get(`/discussion/${discussionId}`);
            setDiscussion(discRes.data);
            fetchComments();
        } catch (err) { console.error(err); }
    };

    const fetchComments = async () => {
        const res = await api.get(`/comment/discussion/${discussionId}`);
        setComments(res.data);
    };

    const CommentNode = ({ comment }: { comment: Comment }) => {
        const [replying, setReplying] = useState(false);
        const [reveal, setReveal] = useState(!comment.isSpoiler);

        return (
            <div className="border-l-2 border-gray-700 pl-4 mt-4">
                <div className="bg-gray-800 p-3 rounded">
                    <div className="flex justify-between items-start">
                        <span className="font-bold text-sm text-blue-400">{comment.userName}</span>
                        <span className="text-xs text-gray-500">{new Date(comment.createdAt).toLocaleString()}</span>
                    </div>

                    {!reveal ? (
                        <button onClick={() => setReveal(true)} className="mt-2 text-red-400 text-sm italic">
                            Warning: Spoiler! Click to reveal.
                        </button>
                    ) : (
                        <p className="mt-1 text-gray-300 text-sm whitespace-pre-wrap">{comment.content}</p>
                    )}

                    <div className="mt-2">
                        {user && (
                            <button onClick={() => setReplying(!replying)} className="text-xs text-gray-400 hover:text-white">Reply</button>
                        )}
                    </div>

                    {replying && <ReplyForm parentId={comment.id} onCancel={() => setReplying(false)} />}
                </div>

                {comment.replies && comment.replies.map(r => <CommentNode key={r.id} comment={r} />)}
            </div>
        );
    };

    if (!discussion) return <p>Loading...</p>;

    return (
        <div className="max-w-4xl mx-auto space-y-8">
            <div className="bg-gray-800 p-6 rounded-lg shadow-lg">
                <h1 className="text-3xl font-bold mb-2">{discussion.title}</h1>
                <div className="text-xs text-gray-500 mb-4 pb-4 border-b border-gray-700">
                    Posted by {discussion.userName} on {new Date(discussion.createdAt).toLocaleDateString()}
                </div>
                <div className="prose prose-invert max-w-none">
                    <p className="whitespace-pre-wrap">{discussion.content}</p>
                </div>
            </div>

            <div>
                <h3 className="text-xl font-bold mb-4">Comments ({comments.length})</h3>
                {user && (
                    <div className="bg-gray-800 p-4 rounded mb-6">
                        <p className="mb-2 text-sm text-gray-400">Leave a comment:</p>
                        <ReplyForm />
                    </div>
                )}

                <div className="space-y-4">
                    {comments.map(c => <CommentNode key={c.id} comment={c} />)}
                </div>
            </div>
        </div>
    );
}
