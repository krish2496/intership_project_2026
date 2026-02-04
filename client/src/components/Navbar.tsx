'use client';

import Link from 'next/link';
import { useRouter } from 'next/navigation';
import { useAuth } from '@/context/AuthContext';

export function Navbar() {
    const { user, logout } = useAuth();
    const router = useRouter();

    return (
        <nav className="bg-gray-800 p-4 shadow-lg">
            <div className="container mx-auto flex justify-between items-center">
                <div className="flex items-center space-x-4">
                    {/* Back Button - Goes to Home */}
                    <Link
                        href="/"
                        className="text-gray-400 hover:text-white transition flex items-center"
                        title="Go to home"
                    >
                        <svg
                            xmlns="http://www.w3.org/2000/svg"
                            className="h-6 w-6"
                            fill="none"
                            viewBox="0 0 24 24"
                            stroke="currentColor"
                        >
                            <path
                                strokeLinecap="round"
                                strokeLinejoin="round"
                                strokeWidth={2}
                                d="M15 19l-7-7 7-7"
                            />
                        </svg>
                    </Link>

                    <Link href="/" className="text-xl font-bold text-blue-500">
                        OtakuTracker
                    </Link>
                </div>

                <div className="flex space-x-4">
                    {user ? (
                        <>
                            <Link href="/dashboard" className="text-gray-300 hover:text-white transition">
                                Dashboard
                            </Link>
                            <Link href="/search" className="text-gray-300 hover:text-white transition">
                                Search
                            </Link>
                            <Link href="/clubs" className="text-gray-300 hover:text-white transition">
                                Clubs
                            </Link>
                            {user.role === 'Admin' && (
                                <Link href="/admin" className="text-red-400 hover:text-red-300 transition">
                                    Admin
                                </Link>
                            )}
                            <Link href="/profile" className="text-gray-300 hover:text-white transition">
                                Profile
                            </Link>
                            <button
                                onClick={logout}
                                className="text-red-400 hover:text-red-300 transition"
                            >
                                Logout
                            </button>
                        </>
                    ) : (
                        <>
                            <Link href="/auth/login" className="text-gray-300 hover:text-white transition">
                                Login
                            </Link>
                            <Link href="/auth/register" className="px-4 py-2 bg-blue-600 rounded text-white hover:bg-blue-700 transition">
                                Sign Up
                            </Link>
                        </>
                    )}
                </div>
            </div>
        </nav>
    );
}
