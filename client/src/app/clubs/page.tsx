'use client';

import { useEffect, useState } from 'react';
import api from '@/lib/api';
import Link from 'next/link';
import { useAuth } from '@/context/AuthContext';
import { toast } from 'react-toastify';
import { useForm } from 'react-hook-form';

interface Club {
    id: number;
    name: string;
    description?: string;
    ownerName: string;
    memberCount: number;
}

export default function ClubsPage() {
    const [clubs, setClubs] = useState<Club[]>([]);
    const [showModal, setShowModal] = useState(false);
    const { user } = useAuth();
    const { register, handleSubmit, reset } = useForm();

    useEffect(() => {
        fetchClubs();
    }, []);

    const fetchClubs = async () => {
        try {
            const response = await api.get('/club');
            setClubs(response.data);
        } catch (err) {
            console.error(err);
        }
    };

    const createClub = async (data: any) => {
        try {
            await api.post('/club', data);
            toast.success('Club created!');
            setShowModal(false);
            reset();
            fetchClubs();
        } catch (err: any) {
            toast.error(err.response?.data || 'Failed to create club');
        }
    };

    return (
        <div className="space-y-6">
            <div className="flex justify-between items-center">
                <h1 className="text-3xl font-bold">Community Clubs</h1>
                {user && (
                    <button
                        onClick={() => setShowModal(true)}
                        className="bg-purple-600 hover:bg-purple-700 text-white px-4 py-2 rounded shadow"
                    >
                        + Create Club
                    </button>
                )}
            </div>

            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
                {clubs.map((club) => (
                    <Link key={club.id} href={`/clubs/${club.id}`} className="block">
                        <div className="bg-gray-800 p-6 rounded-lg shadow-lg hover:shadow-xl transition border border-gray-700 hover:border-purple-500">
                            <h3 className="text-xl font-bold text-white mb-2">{club.name}</h3>
                            <p className="text-gray-400 text-sm mb-4 line-clamp-2">{club.description || 'No description'}</p>
                            <div className="flex justify-between text-xs text-gray-500">
                                <span>Owner: {club.ownerName}</span>
                                <span>Members: {club.memberCount}</span>
                            </div>
                        </div>
                    </Link>
                ))}
                {clubs.length === 0 && <p className="text-gray-500">No clubs found. Be the first to create one!</p>}
            </div>

            {showModal && (
                <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center p-4 z-50">
                    <div className="bg-gray-800 p-8 rounded-lg w-full max-w-md">
                        <h2 className="text-2xl font-bold mb-4">Create a Club</h2>
                        <form onSubmit={handleSubmit(createClub)} className="space-y-4">
                            <div>
                                <label className="block text-sm mb-1">Club Name</label>
                                <input {...register('name', { required: true })} className="w-full p-2 bg-gray-700 rounded text-white" />
                            </div>
                            <div>
                                <label className="block text-sm mb-1">Description</label>
                                <textarea {...register('description')} className="w-full p-2 bg-gray-700 rounded text-white h-24" />
                            </div>
                            <div className="flex justify-end gap-2">
                                <button type="button" onClick={() => setShowModal(false)} className="px-4 py-2 text-gray-400 hover:text-white">Cancel</button>
                                <button type="submit" className="px-4 py-2 bg-purple-600 rounded text-white">Create</button>
                            </div>
                        </form>
                    </div>
                </div>
            )}
        </div>
    );
}
