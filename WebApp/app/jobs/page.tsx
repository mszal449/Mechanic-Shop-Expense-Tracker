import JobsBrowser from '@/components/jobs/JobsBrowser'
import React from 'react'

const JobsPanel = () => {
  return (
    <div>
      <div className='jobs--title'>Browse Jobs</div>
      <JobsBrowser/>
    </div>
  )
}

export default JobsPanel