'use client';

import { useForm } from 'react-hook-form';
import api from '@/lib/api';
import { useAuth } from '@/context/AuthContext';
import { toast } from 'react-toastify';
import Link from 'next/link';

export default function LoginPage() {
    const { register, handleSubmit, formState: { errors } } = useForm();
    const { login } = useAuth();

    const onSubmit = async (data: any) => {
        try {
            const response = await api.post('/auth/login', data);
            login(response.data);
            toast.success('Welcome back!');
        } catch (error: any) {
            toast.error(error.response?.data || 'Login failed');
        }
    };

    return (
        <div className="flex min-h-screen items-center justify-center bg-gray-900">
            <div className="w-full max-w-md p-8 space-y-6 bg-gray-800 rounded-lg shadow-lg">
                <h2 className="text-3xl font-bold text-center text-white">Login</h2>
                <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
                    <div>
                        <label className="block text-sm font-medium text-gray-300">Email</label>
                        <input
                            type="email"
                            {...register('email', { required: 'Email is required' })}
                            className="w-full mt-1 p-2 bg-gray-700 border border-gray-600 rounded text-white focus:outline-none focus:ring-2 focus:ring-blue-500"
                        />
                        {errors.email && <p className="text-red-500 text-xs mt-1">{errors.email.message as string}</p>}
                    </div>

                    <div>
                        <label className="block text-sm font-medium text-gray-300">Password</label>
                        <input
                            type="password"
                            {...register('password', { required: 'Password is required' })}
                            className="w-full mt-1 p-2 bg-gray-700 border border-gray-600 rounded text-white focus:outline-none focus:ring-2 focus:ring-blue-500"
                        />
                        {errors.password && <p className="text-red-500 text-xs mt-1">{errors.password.message as string}</p>}
                    </div>

                    <button
                        type="submit"
                        className="w-full py-2 px-4 bg-green-600 hover:bg-green-700 text-white font-semibold rounded shadow transition duration-200"
                    >
                        Log In
                    </button>
                </form>
                <p className="text-center text-gray-400">
                    Don't have an account? <Link href="/auth/register" className="text-green-400 hover:underline">Sign up</Link>
                </p>
            </div>
        </div>
    );
}
