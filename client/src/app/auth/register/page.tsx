'use client';

import { useForm } from 'react-hook-form';
import api from '@/lib/api';
import { useAuth } from '@/context/AuthContext';
import { toast } from 'react-toastify';
import Link from 'next/link';
import { useRouter } from 'next/navigation';

export default function RegisterPage() {
    const { register, handleSubmit, formState: { errors } } = useForm();
    const { login } = useAuth();
    const router = useRouter();

    const onSubmit = async (data: any) => {
        try {
            const response = await api.post('/auth/register', data);
            login(response.data);
            toast.success('Registration successful!');
        } catch (error: any) {
            toast.error(error.response?.data || 'Registration failed');
        }
    };

    return (
        <div className="flex min-h-screen items-center justify-center bg-gray-900">
            <div className="w-full max-w-md p-8 space-y-6 bg-gray-800 rounded-lg shadow-lg">
                {/* Logo/Brand - Click to go home */}
                <Link href="/" className="block text-center">
                    <h1 className="text-3xl font-bold text-blue-500 hover:text-blue-400 transition-colors">
                        OtakuTracker
                    </h1>
                </Link>

                <h2 className="text-2xl font-bold text-center text-white">Join Tracker</h2>
                <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
                    <div>
                        <label className="block text-sm font-medium text-gray-300">Username</label>
                        <input
                            {...register('username', { required: 'Username is required' })}
                            className="w-full mt-1 p-2 bg-gray-700 border border-gray-600 rounded text-white focus:outline-none focus:ring-2 focus:ring-blue-500"
                        />
                        {errors.username && <p className="text-red-500 text-xs mt-1">{errors.username.message as string}</p>}
                    </div>

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
                            {...register('password', { required: 'Password is required', minLength: { value: 6, message: 'Min 6 chars' } })}
                            className="w-full mt-1 p-2 bg-gray-700 border border-gray-600 rounded text-white focus:outline-none focus:ring-2 focus:ring-blue-500"
                        />
                        {errors.password && <p className="text-red-500 text-xs mt-1">{errors.password.message as string}</p>}
                    </div>

                    <button
                        type="submit"
                        className="w-full py-2 px-4 bg-blue-600 hover:bg-blue-700 text-white font-semibold rounded shadow transition duration-200"
                    >
                        Sign Up
                    </button>
                </form>
                <p className="text-center text-gray-400">
                    Already have an account? <Link href="/auth/login" className="text-blue-400 hover:underline">Log in</Link>
                </p>
            </div>
        </div>
    );
}
