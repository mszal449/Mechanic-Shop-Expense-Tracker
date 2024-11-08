import React from 'react'
import Link from 'next/link'

const Navbar = () => {
    return (
        <div className='p-4 flex justify-between items-center'>
            <div className='flex items-baseline gap-4'>
                <Link href={"/"} className='font-light text-3xl'>Expense Tracker</Link>
                <Link className='navbar--element' href={"/"}>Home</Link>
            </div>
            <div className='flex items-center gap-2'>

            </div>
        </div>
            
    )
}

export default Navbar