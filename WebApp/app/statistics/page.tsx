import React from 'react'
import { ExpenseGraph, JobsSummary } from '@/components'
import { GetJobsStatisticsAsync } from '@/services/jobService';

const Statistics = async () => {
  const {
    totalJobs, 
    pendingJobs, 
    completedJobs, 
    inProgressJobs, 
    cancelledJobs, 
    chartData} = await GetJobsStatisticsAsync();

  return (
    <div className='px-10 mx-40'>
      <div className='text-center text-4xl pb-10'>See statistics of your business</div>

      {/* Count all jobs and their statuses */}
      <JobsSummary
        totalJobs={totalJobs}
        pendingJobs={pendingJobs}
        completedJobs={completedJobs}
        inProgressJobs={inProgressJobs}
        cancelledJobs={cancelledJobs}
      />

      {/* View graph of jobs completed and jobs pending */}
      <ExpenseGraph chartData={chartData}/>
    </div>
  )
}

export default Statistics