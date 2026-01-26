'use client';

import Link from 'next/link';
import { useAuth } from '@/context/AuthContext';

export function Navbar() {
    const { user, logout } = useAuth();

    return (
        <nav className="bg-gray-800 p-4 shadow-lg">
            <div className="container mx-auto flex justify-between items-center">
                <Link href="/" className="text-xl font-bold text-blue-500">
                    OtakuTracker
                </Link>
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
