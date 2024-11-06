import React from 'react'
import { ExpenseGraph, JobsSummary } from '@/components'

const Statistics = () => {
  return (
    <div className='px-10 mx-40'>
      <div className='text-center text-4xl pb-10'>See statistics of your business</div>

      {/* Count all jobs and their statuses */}
      <JobsSummary/>

      {/* View graph of jobs completed and jobs pending */}
      <ExpenseGraph/>
    </div>
  )
}

export default Statistics