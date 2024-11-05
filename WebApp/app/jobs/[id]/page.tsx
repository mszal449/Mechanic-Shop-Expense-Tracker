import { JobPreview } from '@/components'
import React from 'react'



const JobPage = async ({params} : { params: Promise<{id: number}>}) => {
    const jobId = (await params).id

  return (
    <div>
      <JobPreview jobId={jobId}/>
    </div>
  )
}

export default JobPage