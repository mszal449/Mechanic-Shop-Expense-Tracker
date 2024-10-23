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
        <nav className='p-2 flex justify-between items-center'>
            <div className='flex items-baseline'>
                <div className='font-light text-3xl'>Expense Tracker</div>
                <Link className='' href={"/"}>Home</Link>
            </div>
            <div className='flex items-center gap-2'>
                <LoginButton loggedIn={session?.isAuthenticated || null} />

            </div>
        </nav>
            
    )
}

export default Navbar