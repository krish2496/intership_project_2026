'use client';

import { useEffect, useState, use } from 'react';
import api from '@/lib/api';
import Link from 'next/link';
import { useAuth } from '@/context/AuthContext';
import { useForm } from 'react-hook-form';
import { toast } from 'react-toastify';

interface Discussion {
    id: number;
    title: string;
    userName: string;
    createdAt: string;
    isSpoiler: boolean;
}

interface Club {
    id: number;
    name: string;
    description: string;
    ownerName: string;
}

export default function ClubDetailsPage({ params }: { params: Promise<{ id: string }> }) {
    //Unwrap params using React.use()
    const resolvedParams = use(params);
    const id = resolvedParams.id;

    const [club, setClub] = useState<Club | null>(null);
    const [discussions, setDiscussions] = useState<Discussion[]>([]);
    const { user } = useAuth();
    const { register, handleSubmit, reset } = useForm();
    const [showForm, setShowForm] = useState(false);

    useEffect(() => {
        fetchData();
    }, [id]);

    const fetchData = async () => {
        try {
            const clubRes = await api.get(`/club/${id}`);
            setClub(clubRes.data);
            const discRes = await api.get(`/discussion/club/${id}`);
            setDiscussions(discRes.data);
        } catch (err) {
            console.error(err);
        }
    };

    const createDiscussion = async (data: any) => {
        try {
            await api.post(`/discussion/club/${id}`, data);
            toast.success('Topic created!');
            reset();
            setShowForm(false);
            fetchData();
        } catch (err) {
            toast.error('Failed to create topic');
        }
    };

    if (!club) return <p>Loading...</p>;

    return (
        <div className="space-y-6">
            <div className="bg-gray-800 p-8 rounded-lg shadow-lg border-l-4 border-purple-500">
                <h1 className="text-4xl font-bold mb-2">{club.name}</h1>
                <p className="text-gray-300 mb-4">{club.description}</p>
                <p className="text-xs text-gray-500">Managed by {club.ownerName}</p>
            </div>

            <div className="flex justify-between items-center mt-8">
                <h2 className="text-2xl font-bold">Discussions</h2>
                {user && (
                    <button
                        onClick={() => setShowForm(!showForm)}
                        className="bg-blue-600 hover:bg-blue-700 text-white px-4 py-2 rounded text-sm"
                    >
                        {showForm ? 'Cancel' : '+ New Topic'}
                    </button>
                )}
            </div>

            {showForm && (
                <form onSubmit={handleSubmit(createDiscussion)} className="bg-gray-800 p-6 rounded-lg space-y-4 shadow-inner">
                    <input
                        {...register('title', { required: true })}
                        placeholder="Topic Title"
                        className="w-full p-2 bg-gray-700 rounded text-white"
                    />
                    <textarea
                        {...register('content', { required: true })}
                        placeholder="What's on your mind?"
                        className="w-full p-2 bg-gray-700 rounded text-white h-32"
                    />
                    <div className="flex items-center gap-2">
                        <input type="checkbox" {...register('isSpoiler')} id="spoiler" />
                        <label htmlFor="spoiler" className="text-sm">Contains Spoilers?</label>
                    </div>
                    <button type="submit" className="bg-blue-600 px-4 py-2 rounded text-white">Post Topic</button>
                </form>
            )}

            <div className="space-y-4">
                {discussions.map((d) => (
                    <Link key={d.id} href={`/clubs/${id}/discussion/${d.id}`} className="block">
                        <div className={`p-4 rounded-lg flex justify-between items-center transition ${d.isSpoiler ? 'bg-gray-800 border border-red-900' : 'bg-gray-800 hover:bg-gray-750'}`}>
                            <div>
                                <h3 className={`font-bold text-lg ${d.isSpoiler ? 'text-red-400' : 'text-blue-400'}`}>
                                    {d.isSpoiler ? '[SPOILER] ' : ''}{d.title}
                                </h3>
                                <p className="text-xs text-gray-500">Posted by {d.userName} on {new Date(d.createdAt).toLocaleDateString()}</p>
                            </div>
                            <span className="text-gray-400 text-xl">→</span>
                        </div>
                    </Link>
                ))}
                {discussions.length === 0 && <p className="text-gray-500 text-center py-10">No discussions yet.</p>}
            </div>
        </div>
    );
}
