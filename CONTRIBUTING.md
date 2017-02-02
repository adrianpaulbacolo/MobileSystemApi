# Introduction

> We use a variation of the [ENV Workflow][envflow] model. Below is a brief description of our branching and development flow, 
along with references to existing branches on the main Repository and what those branches represent.

## Branch Reference

### Primary Branches

1. **[dev][devbranch]**
> Represents a developmental copy of the UAT Environment. Changes are merged here prior to being deployed on to the UAT server. 

2. **[stage][stagebranch]**
> Represents a working copy of our STG Environment. Changes are merged here prior to being deployed on to the STAGING server.

3. **[master][masterbranch]**
> Represents a working copy of our PROD Environment. Changes are merged here ONLY when a release/deployment is scheduled to be deployed on to the PRODUCTION server. All Pull Requests must be reviewed by [The Boss][bossman] and merged successfully prior to deployment.

4. **[master-api][apibranch]**
> Represents a working copy of the PROD API utilized by various web apps in the Mobile W88 Project. Deployment and merge process should be the same as the master branch in the main repo (see above).

## Projects Reference

1. [W88.m][mwebw88]: W88 Mobile Main Web App

2. [W88.rewards][mrewardsw88]: W88 Mobile Rewards Web App

3. [W88.affiliate][maffiliatew88]: W88 Mobile Affliate Web App

4. [W88.redirect][redirectw88]: Legacy Web App (*note: used for m.w88.com redirect*)

## Development Flow

## Forking

### Guidelines

1. Development should always happen on your forked copy of the [Main Repository][mainrepo]. 
2. Do not clone the Main Repository, instead fork a copy and clone your fork into your local.

## Branching

### Guidelines

1. When a new feature is being built, we create a new branch from '{your-fork}'/master. Development and local testing should happen here so that you are developing against a working copy of PRODUCTION.
2. Feature branch naming is up to you, but please make sure the name describes the feature being built.
3. Feature branches should NOT include work that is unrelated to the feature the branch was created for.
4. Feature branches will ALWAYS have a '{your-fork}/{feature-branch}-dev' AND '{your-fork}/{feature-branch}-stage' counter part. Please see Pull Requests section below for more details.

### Pull Requests

1. When a feature is ready for review, we create a PR from '{your-fork}/{feature-branch}' to 'MainSystemDev/master'. 
    * This is to make sure that your finished work is mergable as-is against a working copy of PRODUCTION. 
    * DO NOT merge this PR your self. ALL pull requests made against 'MainSystemDev/master' must be reviewed and merged by [The Boss][bossman]
    * **NOTE:** Please make sure '{your-fork}/{feature-branch}' is up to date with 'MainSystemDev/master' before creating your initial Pull Request.
2. If the feature is mergable to 'MainSystemDev/master' (see step 1 above), we create another PR to 'MainSystemDev/dev' from '{your-fork}/{feature-branch}-dev' (see step 4 in branching section above)
    * '{your-fork}/{feature-branch}' must be merged into '{your-fork}/{feature-branch}-dev' before creating your PR.
    * This is to make sure that your finished feature is mergable against a working copy of UAT.
    * **NOTE:** Please make sure '{your-fork}/{feature-branch}-dev' is up to date with 'MainSystemDev/dev' before creating your Pull Request.
3. If the feature is mergable to 'MainSystemDev/dev' (see step 2 above), we create another PR to 'MainSystemDev/stage' from '{your-fork}/{feature-branch}-stage' (see step 4 in branching section above)
    * '{your-fork}/{feature-branch}' must be merged into '{your-fork}/{feature-branch}-stage' before creating your PR.
    * This is to make sure that your finished feature is mergable against a working copy of STAGING.
    * **NOTE:** Please make sure '{your-fork}/{feature-branch}-stage' is up to date with 'MainSystemDev/stage' before creating your Pull Request.
4. If the feature has passed UAT and STG merges above and has been approved for PROD deployment, we pass the Pull Request url to [The Boss][bossman] for review and merging. Once the merge is complete, SOP for deployment prep can commence.


[envflow]: https://www.wearefine.com/mingle/env-branching-with-git/
[mainrepo]: https://github.com/MainSystemDev/MobileSystem/
[devbranch]: https://github.com/MainSystemDev/MobileSystem/tree/dev
[stagebranch]: https://github.com/MainSystemDev/MobileSystem/tree/stage
[masterbranch]: https://github.com/MainSystemDev/MobileSystem/tree/master
[apibranch]: https://github.com/MainSystemDev/MobileSystem/tree/master-api
[bossman]: https://github.com/gbmakaveli
[redirectw88]: http://m.w88.com/
[mrewardsw88]: https://mrewards.w88live.com
[maffiliatew88]: https://maffiliate.w88live.com
[mwebw88]: https://m.w88live.com
