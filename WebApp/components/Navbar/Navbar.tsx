'use client'
import React, { useContext, useState } from 'react'
import { authContext } from '@/components/auth/AuthProvider'
import { useRouter } from 'next/navigation'
import { API_ROUTES } from '@/route_config'
import LoginButton from './LoginButton'
import { useAuth } from '../hooks/useAuth'
import Link from 'next/link'

const Navbar = () => {
    const { session } = useAuth();

    return (
        <div className='p-4 flex justify-between items-center'>
            <div className='flex items-baseline gap-4'>
                <Link href={"/"} className='font-light text-3xl'>Expense Tracker</Link>
                <Link className='navbar--element' href={"/"}>Home</Link>
            </div>
            <div className='flex items-center gap-2'>
                <LoginButton loggedIn={session?.isAuthenticated || null} />

            </div>
        </div>
            
    )
}

export default Navbar